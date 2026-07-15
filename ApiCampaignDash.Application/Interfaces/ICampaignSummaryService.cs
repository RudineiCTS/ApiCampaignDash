using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface ICampaignSummaryService
    {
        Task<IEnumerable<CampaignSummaryDto>> GetSummaryAsync(DateTime competenceDateFrom);
        Task<IEnumerable<CampaignResultReportDto>> GetDetailsAsync(int idCampaign);
    }
}
