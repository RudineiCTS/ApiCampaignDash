namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignParams
    {
        public int IdCampaign { get; set; }
        public List<Manufacturer> Manufacturer { get; set; } = new List<Manufacturer>();
        public List<ProductLine> ProductLines { get; set; } = new List<ProductLine>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Clients> Clients { get; set; } = new List<Clients>();
        public int idOrigim { get; set; }
        public bool IsValidSellOutGC { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }



    }
}
