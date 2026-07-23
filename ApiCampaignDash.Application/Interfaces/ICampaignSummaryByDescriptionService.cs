using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface ICampaignSummaryByDescriptionService
    {
        Task<IEnumerable<CampaignSummaryDto>> GetSummaryByDescriptionAsync(DateTime competenceDateFrom, string description);
    }
}
