using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ICampaignSummaryByDescriptionRepository
    {
        Task<IEnumerable<CampaignSummary>> GetSummaryByDescriptionAsync(DateTime competenceDateFrom, string description);
    }
}
