using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ICampaignSummaryRepository
    {
        Task<IEnumerable<CampaignSummary>> GetSummaryAsync(DateTime competenceDateFrom);
    }
}
