namespace ApiCampaignDash.Domain.Entities
{
    public class DynamicReportRow
    {
        public Dictionary<string, object?> Dimensions { get; set; } = new();
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
}
