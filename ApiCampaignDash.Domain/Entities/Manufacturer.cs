using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class Manufacturer
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador do Fabricante")]
        public int IdManufacturer { get; set; }

        [Display(Name = "Nome do Fabricante")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Contém")]
        public string ?IsValid { get; set; } = string.Empty;
    }
}
