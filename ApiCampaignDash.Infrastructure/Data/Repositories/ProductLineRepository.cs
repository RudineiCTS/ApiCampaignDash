using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ProductLineRepository : IProductLineRepository
    {
        private sealed record ProductLineRow(int IdCampaign, int IdProductLine, string? ProductLineName, string? IsValid);

        private readonly AppDbContext _context;

        public ProductLineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductLine>> GetByCampaignIdAsync(int idCampaign)
        {
            var rows = await _context.Database.SqlQuery<ProductLineRow>($"""
                SELECT
                    tblCamTelLin.IDCampanhaTelevendas AS IdCampaign,
                    tblCamTelLin.IDLinhaProduto AS IdProductLine,
                    tblProLin.DescProdutoLinha AS ProductLineName,
                    tblCamTelLin.Contem AS IsValid
                FROM
                    GS300GP.dbo.tblCampanhaTelevendasLinhaProduto tblCamTelLin (NOLOCK)
                LEFT JOIN
                    GS300ERP.dbo.tblProdutoLinha tblProLin (NOLOCK) ON tblProLin.IDProdutoLinha = tblCamTelLin.IDLinhaProduto
                WHERE
                    tblCamTelLin.IDCampanhaTelevendas = {idCampaign}
                """).ToListAsync();

            return rows.Select(r => new ProductLine
            {
                IdCampaign = r.IdCampaign,
                IdProductLine = r.IdProductLine,
                Name = r.ProductLineName ?? string.Empty,
                IsValid = r.IsValid
            });
        }
    }
}
