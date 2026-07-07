using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface ICampaignResultReportService
    {
        Task<IEnumerable<CampaignResultReportDto>> GetResultsAsync(DateTime competenceDateFrom);
    }
}
