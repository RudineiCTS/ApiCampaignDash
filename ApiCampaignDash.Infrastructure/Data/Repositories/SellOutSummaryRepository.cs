using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class SellOutSummaryRepository : ISellOutSummaryRepository
    {
        private sealed record SellOutSummaryRow(decimal SoldValue, int ClientCount);
        private sealed record SellOutMonthlySummaryRow(string YearMonth, decimal SoldValue, int ClientCount);

        private readonly AppDbContext _context;

        public SellOutSummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SellOutSummary> GetTotalsAsync(SellOutSummaryFilter filter)
        {
            var parameters = new List<SqlParameter>();
            var cte = BuildVendasCombinadasCte(filter, parameters);

            var sql = $@"
                {cte}
                SELECT
                    ISNULL(SUM(Total), 0)           AS SoldValue,
                    COUNT(DISTINCT IDPessoaCliente)  AS ClientCount
                FROM
                    VendasCombinadas";

            LogQuery(sql, parameters);

            var result = await _context.Database
                .SqlQueryRaw<SellOutSummaryRow>(sql, parameters.ToArray())
                .SingleAsync();

            return new SellOutSummary
            {
                SoldValue = result.SoldValue,
                ClientCount = result.ClientCount
            };
        }

        public async Task<IEnumerable<SellOutMonthlySummary>> GetMonthlyTotalsAsync(SellOutSummaryFilter filter)
        {
            var parameters = new List<SqlParameter>();
            var cte = BuildVendasCombinadasCte(filter, parameters);

            var sql = $@"
                {cte}
                SELECT
                    FORMAT(DataFaturamento, 'yyyy-MM') AS YearMonth,
                    ISNULL(SUM(Total), 0)              AS SoldValue,
                    COUNT(DISTINCT IDPessoaCliente)     AS ClientCount
                FROM
                    VendasCombinadas
                GROUP BY
                    FORMAT(DataFaturamento, 'yyyy-MM')
                ORDER BY
                    YearMonth";

            LogQuery(sql, parameters);

            var rows = await _context.Database
                .SqlQueryRaw<SellOutMonthlySummaryRow>(sql, parameters.ToArray())
                .ToListAsync();

            return rows.Select(r => new SellOutMonthlySummary
            {
                YearMonth = r.YearMonth,
                SoldValue = r.SoldValue,
                ClientCount = r.ClientCount
            });
        }

        private static string BuildVendasCombinadasCte(SellOutSummaryFilter filter, List<SqlParameter> parameters)
        {
            var manufacturerClause = BuildInClause(filter.IdManufacturer, "fab", parameters);
            var productLineClause = BuildInClause(filter.ProductLine, "lin", parameters);
            var productClause = BuildInClause(filter.Products, "prod", parameters);
            var clientClause = BuildInClause(filter.Clients, "cli", parameters);

            parameters.Add(new SqlParameter("@StartDate", filter.StartDate ?? (object)DBNull.Value));
            parameters.Add(new SqlParameter("@EndDate", filter.EndDate ?? (object)DBNull.Value));
            parameters.Add(new SqlParameter("@IdComissionScenario", filter.IdComissionScenario ?? (object)DBNull.Value));

            return $@"
                ;WITH VendasCombinadas AS
                (
                    /* Canal 1: Televendas */
                    SELECT
                        tblConVen.Total,
                        tblConVen.IDPessoaCliente,
                        tblConVen.DataFaturamento
                    FROM
                        GS300BI.dbo.tblConsolidacaoVendas tblConVen WITH (NOLOCK)
                    WHERE
                        (@StartDate IS NULL OR tblConVen.DataFaturamento >= @StartDate)
                        AND (@EndDate IS NULL OR tblConVen.DataFaturamento <= @EndDate)
                        AND tblConVen.IDOrigem = 1
                        AND tblConVen.IDPessoaGerente <> 159452 /* DANIEL CAMPOS PRESTE */
                        {(manufacturerClause.HasItems ? $"AND tblConVen.IDPessoaFabricante IN ({manufacturerClause.Sql})" : "")}
                        {(productLineClause.HasItems ? $"AND tblConVen.IDProdutoLinha IN ({productLineClause.Sql})" : "")}
                        {(productClause.HasItems ? $"AND tblConVen.IDProduto IN ({productClause.Sql})" : "")}
                        {(clientClause.HasItems ? $"AND tblConVen.IDPessoaCliente IN ({clientClause.Sql})" : "")}
                        AND tblConVen.IDDigitador IN (
                            SELECT tblCamTelBasDig.UsuarioDigitador
                            FROM tblCampanhaTelevendasBaseDigitadoras tblCamTelBasDig WITH (NOLOCK)
                        )

                    UNION ALL

                    /* Canal 2: Bees */
                    SELECT
                        tblConVen.Total,
                        tblConVen.IDPessoaCliente,
                        tblConVen.DataFaturamento
                    FROM
                        GS300BI.dbo.tblConsolidacaoVendas tblConVen WITH (NOLOCK)
                    JOIN
                        tblVendasBees tblVenBees WITH (NOLOCK)
                            ON tblVenBees.IDPedido = tblConVen.IDPedido
                            AND tblVenBees.IDOperacao = tblConVen.IDOperacao
                    LEFT JOIN
                        tblComissaoVendasClienteVendedor tblComVenCliVen WITH (NOLOCK)
                            ON tblComVenCliVen.IDCliente = tblConVen.IDPessoaCliente
                    WHERE
                        (@StartDate IS NULL OR tblConVen.DataFaturamento >= @StartDate)
                        AND (@EndDate IS NULL OR tblConVen.DataFaturamento <= @EndDate)
                        {(manufacturerClause.HasItems ? $"AND tblConVen.IDPessoaFabricante IN ({manufacturerClause.Sql})" : "")}
                        {(productLineClause.HasItems ? $"AND tblConVen.IDProdutoLinha IN ({productLineClause.Sql})" : "")}
                        {(productClause.HasItems ? $"AND tblConVen.IDProduto IN ({productClause.Sql})" : "")}
                        {(clientClause.HasItems ? $"AND tblConVen.IDPessoaCliente IN ({clientClause.Sql})" : "")}
                        {(filter.ConsideraGrandesContas ? "" : "AND tblVenBees.IDDigitador <> 'ce782385-bece-485b-9e33-05ec60591610' /* BRENDA EXCLUSIVA GRANDES CONTAS */")}
                        AND tblComVenCliVen.IDGerente <> 159452 /* DANIEL CAMPOS PRESTE */
                        AND (@IdComissionScenario IS NULL OR tblComVenCliVen.IDComissaoVendasCenario = @IdComissionScenario)
                )";
        }

        private static void LogQuery(string sql, List<SqlParameter> parameters)
        {
            Console.WriteLine(sql);
            Console.WriteLine("Parametros:");
            foreach (var parameter in parameters)
            {
                var value = parameter.Value is null or DBNull ? "NULL" : parameter.Value;
                Console.WriteLine($"  {parameter.ParameterName} = {value}");
            }
        }

        private static (string Sql, bool HasItems) BuildInClause(IEnumerable<int> ids, string prefix, List<SqlParameter> parameters)
        {
            var idList = ids.ToList();
            if (idList.Count == 0)
                return ("", false);

            var paramNames = new List<string>();
            for (int i = 0; i < idList.Count; i++)
            {
                var name = $"@{prefix}{i}";
                parameters.Add(new SqlParameter(name, idList[i]));
                paramNames.Add(name);
            }

            return (string.Join(",", paramNames), true);
        }
    }
}
