using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class DataEntryOperator
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador da Pessoa de Televendas")]
        public int IdPersonSales { get; set; }

        [Display(Name = "Contém")]
        public string? IsValid { get; set; } = string.Empty;
    }
}
