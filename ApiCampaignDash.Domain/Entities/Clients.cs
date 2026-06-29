namespace ApiCampaignDash.Domain.Entities
{
    public class Clients
    {
        public int IdCampaign { get; set; }
        public int IdClients { get; set; }
        public string ? ClientName { get; set; } = string.Empty;
        public string ? IsValid { get; set; } = string.Empty;
    }
}
