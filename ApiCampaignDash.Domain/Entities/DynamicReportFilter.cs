namespace ApiCampaignDash.Domain.Entities
{
    public class DynamicReportFilter
    {
        public string Field { get; set; } = string.Empty;
        public List<string> Values { get; set; } = new();
    }
}
