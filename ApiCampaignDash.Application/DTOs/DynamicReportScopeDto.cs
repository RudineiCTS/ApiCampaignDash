namespace ApiCampaignDash.Application.DTOs
{
    public class DynamicReportScopeDto
    {
        public List<int> IdCampaigns { get; set; } = new();
        public List<int> IdManufacturers { get; set; } = new();
        public List<string> ProductLines { get; set; } = new();
        public List<string> CompetenceMonths { get; set; } = new();
    }
}
