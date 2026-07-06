using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CompetencePeriodStatus
    {
        [Key]
        [Display(Name = "Identificador da Situação do Período de Competência")]
        public int IdCompetencePeriodStatus { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Descrição deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição da Situação do Período de Competência")]
        public string Description { get; set; } = string.Empty;
    }
}
