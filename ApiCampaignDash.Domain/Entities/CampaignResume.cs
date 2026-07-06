using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignResume
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Required(ErrorMessage = "O campo Nome da Campanha é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Nome da Campanha deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome da Campanha")]
        public string TitleCampaign { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Campanha é obrigatório.")]
        [Display(Name = "Tipo de Campanha")]
        public string CampaignType { get; set; }

        [Display(Name = "Meta de Vendas")]
        public decimal SalesTarget { get; set; }

        [Display(Name = "Valor do Prêmio")]
        public decimal AwardValue { get; set; }

        [Display(Name = "Valor Realizado")]
        public decimal ValueRealized { get; set; }

        [Display(Name = "Tipo de Apuração")]
        public int VerificationType { get; set; }

        [Display(Name = "Tipo de Cálculo")]
        public int CalculationType { get; set; }

        [Display(Name = "Válido para Exclusivas")]
        public bool isValidExclusive { get; set; }
    }
}
