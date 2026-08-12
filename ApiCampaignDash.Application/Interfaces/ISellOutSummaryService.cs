using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface ISellOutSummaryService
    {
        Task<SellOutSummaryDto> GetTotalsAsync(SellOutSummaryFilterDto filter);

        Task<IEnumerable<SellOutMonthlySummaryDto>> GetMonthlyTotalsAsync(SellOutSummaryFilterDto filter);
    }
}
