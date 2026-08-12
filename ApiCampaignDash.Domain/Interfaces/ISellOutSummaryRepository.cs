using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ISellOutSummaryRepository
    {
        Task<SellOutSummary> GetTotalsAsync(SellOutSummaryFilter filter);

        Task<IEnumerable<SellOutMonthlySummary>> GetMonthlyTotalsAsync(SellOutSummaryFilter filter);
    }
}
