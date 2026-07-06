using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class Product
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador do Produto")]
        public int IdProduct { get; set; }

        [Display(Name = "Nome do Produto")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Contém")]
        public string ?IsValid { get; set; } = string.Empty;
    }
}
