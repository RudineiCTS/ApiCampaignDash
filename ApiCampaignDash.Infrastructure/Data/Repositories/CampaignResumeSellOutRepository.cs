using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class CampaignResumeSellOutRepository : ICampaignResumeSellOutRepository
    {
        private readonly AppDbContext _context;

        public CampaignResumeSellOutRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CampaignResumeSellOut>> GetCampaignResumeSellOutsAsync(CampaignParams campaign)
        {
         
            var parameters = new List<SqlParameter>();

            var manufacturerClause = BuildInClause(campaign.Manufacturer.Select(m => m.IdManufacturer), "fab", parameters);
            var productLineClause = BuildInClause(campaign.ProductLines.Select(p => p.IdProductLine), "lin", parameters);
            var productClause = BuildInClause(campaign.Products.Select(p => p.IdProduct), "prod", parameters);
            var clientClause = BuildInClause(campaign.Clients.Select(c => c.IdClients), "cli", parameters);
            //parameters.Add(new SqlParameter("@IdCampaign", campaign.IdCampaign));

            parameters.Add(new SqlParameter("@StartDate", campaign.StartDate ?? (object)DBNull.Value));
            parameters.Add(new SqlParameter("@EndDate", campaign.EndDate ?? (object)DBNull.Value));
            parameters.Add(new SqlParameter("@IDOrigem", campaign.idOrigim));

            var sql = $@"
SELECT
    uvwPes.CpfCnpj                          AS CPFCNPJ,
    uvwPes.RazaoSocial                      AS LegalName,
    CAST(tblConVen.IDProduto AS VARCHAR(20)) AS ProductCode,
    vwPro.DescProduto                       AS ProductName,
    vwPro.CodBarras                         AS ProductEan,
    SUM(tblConVen.Quantidade)               AS QuantitySold,
    SUM(tblConVen.Total)                    AS ValueSold,
    uvwPesVen.Nome                          AS SellerName
FROM 
    GS300BI.dbo.tblConsolidacaoVendas AS tblConVen WITH (NOLOCK)
LEFT JOIN GS300ERP.dbo.uvwPessoaFisicaJuridica AS uvwPes WITH (NOLOCK)
    ON uvwPes.IDPessoa = tblConVen.IDPessoaCliente
LEFT JOIN GS300ERP.dbo.vwProduto AS vwPro WITH (NOLOCK)
    ON vwPro.IDProduto = tblConVen.IDProduto
LEFT JOIN GS300ERP.dbo.uvwPessoaFisicaJuridica AS uvwPesVen WITH (NOLOCK)
    ON uvwPesVen.IDPessoa = tblConVen.IDPessoaVendedor
WHERE 
    tblConVen.DataFaturamento BETWEEN @StartDate AND @EndDate
  AND tblConVen.IDDigitador IN (SELECT UsuarioDigitador FROM tblCampanhaTelevendasBaseDigitadoras WITH (NOLOCK))
  AND tblConVen.IDOrigem = @IDOrigem
  {(manufacturerClause.HasItems ? $"AND tblConVen.IDPessoaFabricante IN ({manufacturerClause.Sql})" : "")}
  {(productLineClause.HasItems ? $"AND tblConVen.IDProdutoLinha IN ({productLineClause.Sql})" : "")}
  {(productClause.HasItems ? $"AND tblConVen.IDProduto IN ({productClause.Sql})" : "")}
  {(clientClause.HasItems ? $"AND tblConVen.IDPessoaCliente IN ({clientClause.Sql})" : "")}
GROUP BY
    uvwPes.CpfCnpj, uvwPes.RazaoSocial, tblConVen.IDProduto,
    vwPro.CodBarras, uvwPesVen.Nome, vwPro.DescProduto

UNION

SELECT
    uvwPes.CpfCnpj                          AS CPFCNPJ,
    uvwPes.RazaoSocial                      AS LegalName,
    CAST(tblConVen.IDProduto AS VARCHAR(20)) AS ProductCode,
    vwPro.DescProduto                       AS ProductName,
    vwPro.CodBarras                         AS ProductEan,
    SUM(tblConVen.Quantidade)               AS QuantitySold,
    SUM(tblConVen.Total)                    AS ValueSold,
    tblSegUsu.NomeUsuario                   AS SellerName
FROM GS300BI.dbo.tblConsolidacaoVendas AS tblConVen WITH (NOLOCK)
JOIN tblVendasBees AS tblVenBees WITH (NOLOCK)
    ON tblVenBees.IDPedido = tblConVen.IDPedido AND tblVenBees.IDOperacao = tblConVen.IDOperacao
LEFT JOIN tblComissaoVendasClienteVendedor AS tblComVenCliVen WITH (NOLOCK)
    ON tblComVenCliVen.IDCliente = tblConVen.IDPessoaCliente
LEFT JOIN GS300ERP.dbo.uvwPessoaFisicaJuridica AS uvwPes WITH (NOLOCK)
    ON uvwPes.IDPessoa = tblConVen.IDPessoaCliente
LEFT JOIN GS300ERP.dbo.vwProduto AS vwPro WITH (NOLOCK)
    ON vwPro.IDProduto = tblConVen.IDProduto
LEFT JOIN GS300ERP.dbo.tblSegUsuario AS tblSegUsu WITH (NOLOCK)
    ON tblVenBees.IDDigitador = tblSegUsu.IDUsuario
WHERE tblConVen.DataFaturamento >= @StartDate
  AND tblConVen.DataFaturamento < DATEADD(DAY, 1, @EndDate)
  {(manufacturerClause.HasItems ? $"AND tblConVen.IDPessoaFabricante IN ({manufacturerClause.Sql})" : "")}
  {(productLineClause.HasItems ? $"AND tblConVen.IDProdutoLinha IN ({productLineClause.Sql})" : "")}
  {(productClause.HasItems ? $"AND tblConVen.IDProduto IN ({productClause.Sql})" : "")}
  {(clientClause.HasItems ? $"AND tblConVen.IDPessoaCliente IN ({clientClause.Sql})" : "")}
  AND tblVenBees.IDDigitador <> 'ce782385-bece-485b-9e33-05ec60591610' /* BRENDA EXCLUSIVA GRANDES CONTAS */
  AND tblComVenCliVen.IDGerente <> 159452 /* DANIEL CAMPOS PRESTE */
  AND tblComVenCliVen.IDComissaoVendasCenario = 100
GROUP BY
    uvwPes.CpfCnpj, uvwPes.RazaoSocial, tblConVen.IDProduto,
    vwPro.DescProduto, vwPro.CodBarras, tblSegUsu.NomeUsuario";

            Console.WriteLine(sql);
            return await _context.Database.SqlQueryRaw<CampaignResumeSellOut>(sql, parameters.ToArray())
                .ToListAsync();
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
