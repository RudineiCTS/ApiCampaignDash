namespace ApiCampaignDash.Domain.Entities
{
    public class DataEntryOperator
    {
        public int IdCampaign { get; set; }
        public int IdPersonSales { get; set; }
        public string? IsValid { get; set; } = string.Empty;
    }
}
