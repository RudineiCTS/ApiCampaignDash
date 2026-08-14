using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface IDynamicReportService
    {
        Task<DynamicReportResponseDto> GetReportAsync(DynamicReportRequestDto request);
    }
}
