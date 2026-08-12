namespace ApiCampaignDash.Application.DTOs
{
    public class SellOutMonthlySummaryDto
    {
        public string YearMonth { get; set; } = string.Empty;
        public decimal SoldValue { get; set; }
        public int ClientCount { get; set; }
    }
}
