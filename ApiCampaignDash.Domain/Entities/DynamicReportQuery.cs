namespace ApiCampaignDash.Domain.Entities
{
    public class DynamicReportQuery
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<int> ManufacturerIds { get; set; } = new();
        public List<int> ProductLineIds { get; set; } = new();
        public List<int> ProductIds { get; set; } = new();
        public List<int> ClientIds { get; set; } = new();

        public int? IdCampaign { get; set; }
        public string? CampaignName { get; set; }

        public List<string> CompetenceMonths { get; set; } = new();

        public List<string> GroupBy { get; set; } = new();
        public List<string> Metrics { get; set; } = new();
        public List<DynamicReportFilter> Filters { get; set; } = new();
    }
}
