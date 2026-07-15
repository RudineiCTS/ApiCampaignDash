using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ICampaignSummaryRepository
    {
        Task<IEnumerable<CampaignSummary>> GetSummaryAsync(DateTime competenceDateFrom);
        Task<IEnumerable<CampaignResultReport>> GetDetailsAsync(int idCampaign);
    }
}
