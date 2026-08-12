namespace ApiCampaignDash.Application.DTOs
{
    public class SellOutSummaryFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> IdManufacturer { get; set; } = new();
        public List<int> ProductLine { get; set; } = new();
        public List<int> Products { get; set; } = new();
        public int? IdComissionScenario { get; set; }
        public List<int> Clients { get; set; } = new();
        public bool ConsideraGrandesContas { get; set; }
    }
}
