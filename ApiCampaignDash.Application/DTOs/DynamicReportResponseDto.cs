namespace ApiCampaignDash.Application.DTOs
{
    public class DynamicReportResponseDto
    {
        public List<DynamicReportRowDto> Rows { get; set; } = new();
        public Dictionary<string, decimal> Totals { get; set; } = new();
        public int GroupCount { get; set; }
    }
}
