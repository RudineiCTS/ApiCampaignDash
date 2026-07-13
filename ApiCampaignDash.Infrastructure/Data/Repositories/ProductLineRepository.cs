using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ProductLineRepository : IProductLineRepository
    {
        private readonly AppDbContext _context;

        public ProductLineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductLine>> GetByCampaignIdAsync(int idCampaign)
        {
            return await _context.ProductLines
                .AsNoTracking()
                .Where(p => p.IdCampaign == idCampaign)
                .ToListAsync();
        }
    }
}
