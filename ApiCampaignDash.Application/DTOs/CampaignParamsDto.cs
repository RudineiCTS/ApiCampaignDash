namespace ApiCampaignDash.Application.DTOs
{
    public class CampaignParamsDto
    {
        public int IdCampaign { get; set; }
        public List<ManufacturerDto> Manufacturer { get; set; } = new List<ManufacturerDto>();
        public List<ProductLineDto> ProductLines { get; set; } = new List<ProductLineDto>();
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
        public List<ClientsDto> Clients { get; set; } = new List<ClientsDto>();
        public int idOrigim { get; set; }
        public bool IsValidSellOutGC { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
