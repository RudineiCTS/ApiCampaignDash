using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ICampaignResultReportRepository
    {
        Task<IEnumerable<CampaignResultReport>> GetResultsAsync(DateTime competenceDateFrom);
    }
}
