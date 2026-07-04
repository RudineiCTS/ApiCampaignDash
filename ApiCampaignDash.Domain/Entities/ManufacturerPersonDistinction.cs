namespace ApiCampaignDash.Domain.Entities
{
    public class ManufacturerPersonDistinction
    {
        public int IdManufacturer { get; set; }
        public string ManufacturerDescription { get; set; } = string.Empty;
        public int IdPerson { get; set; }
        public int IdOperator { get; set; }
    }
}
