namespace ApiCampaignDash.Domain.Entities
{
    public class Manufacturer
    {
        public int IdCampaign { get; set; }
        public int IdManufacturer { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ?IsValid { get; set; } = string.Empty;
    }
}
