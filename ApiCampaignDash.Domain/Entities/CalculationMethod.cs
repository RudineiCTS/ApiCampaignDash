using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CalculationMethod
    {
        [Key]
        [Display(Name = "Identificador do Tipo de Cálculo")]
        public int IdCalculationMethod { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Descrição deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; } = string.Empty;
    }
}
