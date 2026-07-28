using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private sealed record ProductRow(int IdCampaign, int IdProduct, string? ProductName, string? IsValid);

        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetByCampaignIdAsync(int idCampaign)
        {
            var rows = await _context.Database.SqlQuery<ProductRow>($"""
                SELECT
                    tblCamTelPro.IDCampanhaTelevendas AS IdCampaign,
                    tblCamTelPro.IDProduto AS IdProduct,
                    tblPro.DescProduto AS ProductName,
                    tblCamTelPro.Contem AS IsValid
                FROM
                    GS300GP.dbo.tblCampanhaTelevendasProduto tblCamTelPro (NOLOCK)
                LEFT JOIN
                    GS300ERP.dbo.tblProduto tblPro (NOLOCK) ON tblPro.IDProduto = tblCamTelPro.IDProduto
                WHERE
                    tblCamTelPro.IDCampanhaTelevendas = {idCampaign}
                """).ToListAsync();

            return rows.Select(r => new Product
            {
                IdCampaign = r.IdCampaign,
                IdProduct = r.IdProduct,
                Name = r.ProductName ?? string.Empty,
                IsValid = r.IsValid
            });
        }
    }
}
