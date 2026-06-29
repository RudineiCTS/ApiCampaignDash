namespace ApiCampaignDash.Domain.Entities
{
    public class Product
    {
        public int IdCampaign { get; set; }
        public int IdProduct { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ?IsValid { get; set; } = string.Empty;
    }
}
