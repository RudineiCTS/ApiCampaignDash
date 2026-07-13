namespace ApiCampaignDash.Application.DTOs
{
    public class ManufacturerDto
    {
        public int IdCampaign { get; set; }
        public int IdManufacturer { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? IsValid { get; set; }
    }
}
