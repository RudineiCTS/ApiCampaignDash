namespace ApiCampaignDash.Application.DTOs
{
    public class ClientsDto
    {
        public int IdCampaign { get; set; }
        public int IdClients { get; set; }
        public string? ClientName { get; set; }
        public string? CpfCnpj { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? IsValid { get; set; }
    }
}
