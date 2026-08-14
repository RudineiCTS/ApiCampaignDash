namespace ApiCampaignDash.Domain.Entities
{
    public class DynamicReportResult
    {
        public List<DynamicReportRow> Rows { get; set; } = new();
        public Dictionary<string, decimal> Totals { get; set; } = new();
        public int GroupCount { get; set; }
    }
}
