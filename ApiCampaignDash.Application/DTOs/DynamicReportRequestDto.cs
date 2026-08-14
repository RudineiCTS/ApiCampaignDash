namespace ApiCampaignDash.Application.DTOs
{
    public class DynamicReportRequestDto
    {
        public DynamicReportScopeDto Scope { get; set; } = new();
        public List<string> GroupBy { get; set; } = new();
        public List<string> Columns { get; set; } = new();
        public List<string> Metrics { get; set; } = new();
        public List<DynamicReportFilterDto> Filters { get; set; } = new();
    }
}
