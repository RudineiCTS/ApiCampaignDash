namespace ApiCampaignDash.Application.DTOs
{
    public class ProductLineDto
    {
        public int IdCampaign { get; set; }
        public int IdProductLine { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IsValid { get; set; }
    }
}
