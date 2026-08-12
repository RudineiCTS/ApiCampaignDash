namespace ApiCampaignDash.Domain.Entities
{
    public class SellOutMonthlySummary
    {
        public string YearMonth { get; set; } = string.Empty;
        public decimal SoldValue { get; set; }
        public int ClientCount { get; set; }
    }
}
