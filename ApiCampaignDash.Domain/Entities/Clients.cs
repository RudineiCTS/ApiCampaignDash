using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class Clients
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador do Cliente")]
        public int IdClients { get; set; }

        [Display(Name = "Nome do Cliente")]
        public string ? ClientName { get; set; } = string.Empty;

        [Display(Name = "Contém")]
        public string ? IsValid { get; set; } = string.Empty;
    }
}
