using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class ProductLine
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador da Linha de Produto")]
        public int IdProductLine { get; set; }

        [Display(Name = "Nome da Linha de Produto")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Contém")]
        public string ?IsValid { get; set; }
    }
}
