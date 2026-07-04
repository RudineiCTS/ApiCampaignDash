using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly AppDbContext _context;

        public CampaignRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<IEnumerable<Campaign>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Campaign?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Campaign>> GetByPeriodCampaignAsync(DateTime datetime)
        {
            throw new NotImplementedException();
        }
    }
}
