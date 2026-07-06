using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class Campaign
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Display(Name = "Data de Competência")]
        public DateTime? CompetenceDate { get; set; }

        [Required(ErrorMessage = "O campo Situação do Período de Competência é obrigatório.")]
        [Display(Name = "Situação do Período de Competência")]
        public int IdCompetencePeriodStatus { get; set; }

        [Display(Name = "Data de Início da Apuração")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Data de Fim da Apuração")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Total de Ranking")]
        public int? TotalRanking { get; set; }

        [Required(ErrorMessage = "O campo Descrição da Campanha é obrigatório.")]
        [StringLength(150, ErrorMessage = "O campo Descrição da Campanha deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição da Campanha")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Tipo de Apuração é obrigatório.")]
        [Display(Name = "Tipo de Apuração")]
        public int IdAssessmentType { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Cálculo é obrigatório.")]
        [Display(Name = "Tipo de Cálculo")]
        public int IdCalculationMethod { get; set; }

        [Display(Name = "Regra de Validação")]
        public int? ValidationRule { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Valor é obrigatório.")]
        [Display(Name = "Tipo de Valor")]
        public int ValueType { get; set; }

        [Display(Name = "Data de Fim Antecipada")]
        public DateTime? EarlyEndDate { get; set; }

        [StringLength(150, ErrorMessage = "O campo Observação deve ter no máximo {1} caracteres.")]
        [Display(Name = "Observação")]
        public string? Notes { get; set; }

        [Display(Name = "Considera Exclusivas")]
        public bool? ConsidersExclusives { get; set; }

        [Display(Name = "Tipo de Campanha")]
        public bool? CampaignType { get; set; }
    }
}
