using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetByCampaignIdAsync(int idCampaign)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IdCampaign == idCampaign)
                .ToListAsync();
        }
    }
}
