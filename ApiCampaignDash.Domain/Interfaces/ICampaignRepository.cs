using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ICampaignRepository : IBaseRepository<Campaign>
    {
        Task<IEnumerable<Campaign>> GetByPeriodCampaignAsync(DateTime datetime);

    }
}
