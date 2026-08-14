using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IDynamicReportRepository
    {
        Task<DynamicReportResult> GetReportAsync(DynamicReportQuery query);
    }
}
