namespace ApiCampaignDash.Application.DTOs
{
    public class DynamicReportFilterDto
    {
        public string Field { get; set; } = string.Empty;
        public List<string> Values { get; set; } = new();
    }
}
