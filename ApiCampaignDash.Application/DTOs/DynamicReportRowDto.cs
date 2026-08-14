namespace ApiCampaignDash.Application.DTOs
{
    public class DynamicReportRowDto
    {
        public Dictionary<string, object?> Dimensions { get; set; } = new();
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
}
