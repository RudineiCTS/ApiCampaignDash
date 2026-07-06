using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Campaign>> GetByPeriodCampaignAsync(DateTime datetime)
        {
            return await _context.Campaigns
                .AsNoTracking()
                .Where(c => c.CompetenceDate == datetime)
                .ToListAsync();
        }
    }
}
