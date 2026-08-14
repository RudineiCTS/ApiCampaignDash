using System.Data;
using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class DynamicReportRepository : IDynamicReportRepository
    {
        private static readonly Dictionary<string, string> DimensionColumns = new()
        {
            ["IDCampanha"] = "IDCampanha",
            ["NomeCampanha"] = "NomeCampanha",
            ["IDFabricante"] = "IDFabricante",
            ["NomeFabricante"] = "NomeFabricante",
            ["Linha"] = "Linha",
            ["Origem"] = "Origem",
            ["IDVendedor"] = "Vendedor",
            ["IDSupervisor"] = "Supervisor",
            ["IDGerente"] = "Gerente",
            ["DataCompetencia"] = "DataCompetencia",
        };

        private static readonly Dictionary<string, string> MetricExpressions = new()
        {
            ["ValorVendido"] = "SUM(Total)",
            ["Positivacao"] = "COUNT(DISTINCT IDPessoaCliente)",
        };

        private readonly AppDbContext _context;

        public DynamicReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DynamicReportResult> GetReportAsync(DynamicReportQuery query)
        {
            var paramValues = new List<(string Name, object Value)>();

            var manufacturerClause = BuildInClause(query.ManufacturerIds, "fab", paramValues);
            var productLineClause = BuildInClause(query.ProductLineIds, "lin", paramValues);
            var productClause = BuildInClause(query.ProductIds, "prod", paramValues);
            var clientClause = BuildInClause(query.ClientIds, "cli", paramValues);

            paramValues.Add(("@StartDate", (object?)query.StartDate ?? DBNull.Value));
            paramValues.Add(("@EndDate", (object?)query.EndDate ?? DBNull.Value));
            paramValues.Add(("@IdCampanha", (object?)query.IdCampaign ?? DBNull.Value));
            paramValues.Add(("@NomeCampanha", (object?)query.CampaignName ?? DBNull.Value));

            var cte = BuildVendasBaseCte(manufacturerClause, productLineClause, productClause, clientClause);

            var filterClauses = new List<string>();
            foreach (var filter in query.Filters)
            {
                if (!DimensionColumns.TryGetValue(filter.Field, out var column)) continue;
                var inClause = BuildInClause(filter.Values, $"flt{filterClauses.Count}", paramValues);
                if (inClause.HasItems)
                    filterClauses.Add($"CAST({column} AS NVARCHAR(200)) IN ({inClause.Sql})");
            }

            if (query.CompetenceMonths.Count > 0)
            {
                var cmClause = BuildInClause(query.CompetenceMonths, "cm", paramValues);
                if (cmClause.HasItems)
                    filterClauses.Add($"DataCompetencia IN ({cmClause.Sql})");
            }

            var whereClause = filterClauses.Count > 0 ? $"WHERE {string.Join(" AND ", filterClauses)}" : "";

            var dimMappings = query.GroupBy.Select(g => (FieldKey: g, Column: DimensionColumns[g])).ToList();
            var metricMappings = query.Metrics.Select(m => (FieldKey: m, Expr: MetricExpressions[m])).ToList();

            var rowsSql = BuildSelectSql(cte, whereClause, dimMappings, metricMappings, includeGroupBy: true);
            var totalsSql = BuildSelectSql(cte, whereClause, new(), metricMappings, includeGroupBy: false);

            var connection = _context.Database.GetDbConnection();
            var shouldClose = connection.State != ConnectionState.Open;
            if (shouldClose) await connection.OpenAsync();

            try
            {
                var rows = await ExecuteRowsAsync(connection, rowsSql, paramValues, dimMappings, metricMappings);
                var totals = await ExecuteTotalsAsync(connection, totalsSql, paramValues, metricMappings);

                return new DynamicReportResult
                {
                    Rows = rows,
                    Totals = totals,
                    GroupCount = rows.Count,
                };
            }
            finally
            {
                if (shouldClose) await connection.CloseAsync();
            }
        }

        private static string BuildSelectSql(
            string cte,
            string whereClause,
            List<(string FieldKey, string Column)> dimMappings,
            List<(string FieldKey, string Expr)> metricMappings,
            bool includeGroupBy)
        {
            var selectParts = dimMappings
                .Select(d => $"{d.Column} AS [{d.FieldKey}]")
                .Concat(metricMappings.Select(m => $"{m.Expr} AS [{m.FieldKey}]"))
                .ToList();

            var groupByClause = includeGroupBy && dimMappings.Count > 0
                ? $"GROUP BY {string.Join(", ", dimMappings.Select(d => d.Column))}"
                : "";

            return $@"
                {cte}
                SELECT
                    {string.Join(",\n                    ", selectParts)}
                FROM VendasBase
                {whereClause}
                {groupByClause}";
        }

        private static async Task<List<DynamicReportRow>> ExecuteRowsAsync(
            System.Data.Common.DbConnection connection,
            string sql,
            List<(string Name, object Value)> paramValues,
            List<(string FieldKey, string Column)> dimMappings,
            List<(string FieldKey, string Expr)> metricMappings)
        {
            var rows = new List<DynamicReportRow>();

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = 120;
            AddParameters(command, paramValues);

            Console.WriteLine(sql);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new DynamicReportRow();

                foreach (var dim in dimMappings)
                {
                    var value = reader[dim.FieldKey];
                    row.Dimensions[dim.FieldKey] = value is DBNull ? null : value;
                }

                foreach (var metric in metricMappings)
                {
                    var value = reader[metric.FieldKey];
                    row.Metrics[metric.FieldKey] = value is DBNull ? 0m : Convert.ToDecimal(value);
                }

                rows.Add(row);
            }

            return rows;
        }

        private static async Task<Dictionary<string, decimal>> ExecuteTotalsAsync(
            System.Data.Common.DbConnection connection,
            string sql,
            List<(string Name, object Value)> paramValues,
            List<(string FieldKey, string Expr)> metricMappings)
        {
            var totals = metricMappings.ToDictionary(m => m.FieldKey, _ => 0m);

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = 120;
            AddParameters(command, paramValues);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                foreach (var metric in metricMappings)
                {
                    var value = reader[metric.FieldKey];
                    totals[metric.FieldKey] = value is DBNull ? 0m : Convert.ToDecimal(value);
                }
            }

            return totals;
        }

        private static void AddParameters(IDbCommand command, List<(string Name, object Value)> paramValues)
        {
            foreach (var (name, value) in paramValues)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = name;
                parameter.Value = value;
                command.Parameters.Add(parameter);
            }
        }

        private static string BuildVendasBaseCte(
            (string Sql, bool HasItems) manufacturerClause,
            (string Sql, bool HasItems) productLineClause,
            (string Sql, bool HasItems) productClause,
            (string Sql, bool HasItems) clientClause)
        {
            return $@"
                ;WITH VendasBase AS
                (
                    /* Canal 1: Televendas */
                    SELECT
                        'Televendas'                            AS Origem,
                        @IdCampanha                              AS IDCampanha,
                        @NomeCampanha                            AS NomeCampanha,
                        tblConVen.IDPessoaFabricante             AS IDFabricante,
                        uvwPesFab.Nome                           AS NomeFabricante,
                        tblProLin.DescProdutoLinha               AS Linha,
                        tblCamTelBasDig.NomeDigitador            AS Vendedor,
                        tblCamTelBasDig.NomeSupervisor           AS Supervisor,
                        uvwPesGer.Nome                           AS Gerente,
                        FORMAT(tblConVen.DataFaturamento, 'yyyy-MM') AS DataCompetencia,
                        tblConVen.IDPessoaCliente                AS IDPessoaCliente,
                        tblConVen.Total                          AS Total
                    FROM
                        GS300BI.dbo.tblConsolidacaoVendas tblConVen WITH (NOLOCK)
                    LEFT JOIN
                        GS300ERP.dbo.uvwPessoaFisicaJuridica uvwPesFab WITH (NOLOCK) ON uvwPesFab.IDPessoa = tblConVen.IDPessoaFabricante
                    LEFT JOIN
                        GS300ERP.dbo.tblProdutoLinha tblProLin WITH (NOLOCK) ON tblProLin.IDProdutoLinha = tblConVen.IDProdutoLinha
                    LEFT JOIN
                        tblCampanhaTelevendasBaseDigitadoras tblCamTelBasDig WITH (NOLOCK) ON tblCamTelBasDig.UsuarioDigitador = tblConVen.IDDigitador
                    LEFT JOIN
                        tblComissaoVendasClienteVendedor tblComVenCliVen WITH (NOLOCK) ON tblComVenCliVen.IDCliente = tblConVen.IDPessoaCliente
                    LEFT JOIN
                        GS300ERP.dbo.uvwPessoaFisicaJuridica uvwPesGer WITH (NOLOCK) ON uvwPesGer.IDPessoa = tblComVenCliVen.IDGerente
                    WHERE
                        (@StartDate IS NULL OR tblConVen.DataFaturamento >= @StartDate)
                        AND (@EndDate IS NULL OR tblConVen.DataFaturamento <= @EndDate)
                        AND tblConVen.IDOrigem = 1
                        AND tblConVen.IDDigitador IN (
                            SELECT tblCamTelBasDig2.UsuarioDigitador
                            FROM tblCampanhaTelevendasBaseDigitadoras tblCamTelBasDig2 WITH (NOLOCK)
                        )
                        {(manufacturerClause.HasItems ? $"AND tblConVen.IDPessoaFabricante IN ({manufacturerClause.Sql})" : "")}
                        {(productLineClause.HasItems ? $"AND tblConVen.IDProdutoLinha IN ({productLineClause.Sql})" : "")}
                        {(productClause.HasItems ? $"AND tblConVen.IDProduto IN ({productClause.Sql})" : "")}
                        {(clientClause.HasItems ? $"AND tblConVen.IDPessoaCliente IN ({clientClause.Sql})" : "")}
                        AND (tblComVenCliVen.IDGerente IS NULL OR tblComVenCliVen.IDGerente <> 159452) /* DANIEL CAMPOS PRESTE */

                    UNION ALL

                    /* Canal 2: Bees */
                    SELECT
                        'Bees'                                   AS Origem,
                        @IdCampanha                              AS IDCampanha,
                        @NomeCampanha                            AS NomeCampanha,
                        tblConVen.IDPessoaFabricante             AS IDFabricante,
                        uvwPesFab.Nome                           AS NomeFabricante,
                        tblProLin.DescProdutoLinha               AS Linha,
                        tblSegUsu.NomeUsuario                    AS Vendedor,
                        NULL                                     AS Supervisor,
                        uvwPesGer.Nome                           AS Gerente,
                        FORMAT(tblConVen.DataFaturamento, 'yyyy-MM') AS DataCompetencia,
                        tblConVen.IDPessoaCliente                AS IDPessoaCliente,
                        tblConVen.Total                          AS Total
                    FROM
                        GS300BI.dbo.tblConsolidacaoVendas tblConVen WITH (NOLOCK)
                    JOIN
                        tblVendasBees tblVenBees WITH (NOLOCK)
                            ON tblVenBees.IDPedido = tblConVen.IDPedido
                            AND tblVenBees.IDOperacao = tblConVen.IDOperacao
                    LEFT JOIN
                        GS300ERP.dbo.uvwPessoaFisicaJuridica uvwPesFab WITH (NOLOCK) ON uvwPesFab.IDPessoa = tblConVen.IDPessoaFabricante
                    LEFT JOIN
                        GS300ERP.dbo.tblProdutoLinha tblProLin WITH (NOLOCK) ON tblProLin.IDProdutoLinha = tblConVen.IDProdutoLinha
                    LEFT JOIN
                        GS300ERP.dbo.tblSegUsuario tblSegUsu WITH (NOLOCK) ON tblSegUsu.IDUsuario = tblVenBees.IDDigitador
                    LEFT JOIN
                        tblComissaoVendasClienteVendedor tblComVenCliVen WITH (NOLOCK) ON tblComVenCliVen.IDCliente = tblConVen.IDPessoaCliente
                    LEFT JOIN
                        GS300ERP.dbo.uvwPessoaFisicaJuridica uvwPesGer WITH (NOLOCK) ON uvwPesGer.IDPessoa = tblComVenCliVen.IDGerente
                    WHERE
                        (@StartDate IS NULL OR tblConVen.DataFaturamento >= @StartDate)
                        AND (@EndDate IS NULL OR tblConVen.DataFaturamento < DATEADD(DAY, 1, @EndDate))
                        {(manufacturerClause.HasItems ? $"AND tblConVen.IDPessoaFabricante IN ({manufacturerClause.Sql})" : "")}
                        {(productLineClause.HasItems ? $"AND tblConVen.IDProdutoLinha IN ({productLineClause.Sql})" : "")}
                        {(productClause.HasItems ? $"AND tblConVen.IDProduto IN ({productClause.Sql})" : "")}
                        {(clientClause.HasItems ? $"AND tblConVen.IDPessoaCliente IN ({clientClause.Sql})" : "")}
                        AND tblVenBees.IDDigitador <> 'ce782385-bece-485b-9e33-05ec60591610' /* BRENDA EXCLUSIVA GRANDES CONTAS */
                        AND (tblComVenCliVen.IDGerente IS NULL OR tblComVenCliVen.IDGerente <> 159452) /* DANIEL CAMPOS PRESTE */
                )";
        }

        private static (string Sql, bool HasItems) BuildInClause(IEnumerable<int> ids, string prefix, List<(string Name, object Value)> paramValues)
        {
            var idList = ids.ToList();
            if (idList.Count == 0)
                return ("", false);

            var paramNames = new List<string>();
            for (int i = 0; i < idList.Count; i++)
            {
                var name = $"@{prefix}{i}";
                paramValues.Add((name, idList[i]));
                paramNames.Add(name);
            }

            return (string.Join(",", paramNames), true);
        }

        private static (string Sql, bool HasItems) BuildInClause(IEnumerable<string> values, string prefix, List<(string Name, object Value)> paramValues)
        {
            var valueList = values.Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
            if (valueList.Count == 0)
                return ("", false);

            var paramNames = new List<string>();
            for (int i = 0; i < valueList.Count; i++)
            {
                var name = $"@{prefix}{i}";
                paramValues.Add((name, valueList[i]));
                paramNames.Add(name);
            }

            return (string.Join(",", paramNames), true);
        }
    }
}
