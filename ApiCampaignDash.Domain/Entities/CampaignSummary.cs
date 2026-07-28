using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignSummary
    {
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição da Campanha")]
        public string? CampaignDescription { get; set; }

        [Display(Name = "Data de Competência")]
        public DateTime? CompetenceDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Tipo de Campanha")]
        public string? CampaignTypeDescription { get; set; }

        [Display(Name = "Valor da Meta")]
        public decimal? GoalValue { get; set; }

        [Display(Name = "Valor Apurado")]
        public decimal? AssessedValue { get; set; }

        [Display(Name = "Valor Apurado Bees")]
        public decimal? AssessedValueBees { get; set; }

        [Display(Name = "Premiação Total")]
        public decimal? TotalAward { get; set; }

        [Display(Name = "Total do Pote")]
        public decimal? TotalPot { get; set; }

        [Display(Name = "Percentual Realizado")]
        public decimal? PercentageAchieved { get; set; }

        [StringLength(150)]
        [Display(Name = "Observação")]
        public string? Notes { get; set; }

        [StringLength(50)]
        [Display(Name = "Tipo de Campanha")]
        public string? TypeCampaign { get; set; }

        [Display(Name = "Dinâmica")]
        public bool? IsDynamic { get; set; }

        [Display(Name = "Situação do Período de Competência")]
        public int IdCompetencePeriodStatus { get; set; }

        [Display(Name = "Data de Início da Apuração")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Data de Fim da Apuração")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Total de Ranking")]
        public int? TotalRanking { get; set; }

        [Display(Name = "Tipo de Apuração")]
        public int IdAssessmentType { get; set; }

        [Display(Name = "Tipo de Cálculo")]
        public int IdCalculationMethod { get; set; }

        [Display(Name = "Regra de Validação")]
        public int? ValidationRule { get; set; }

        [Display(Name = "Tipo de Valor")]
        public int ValueType { get; set; }

        [Display(Name = "Data de Fim Antecipada")]
        public DateTime? EarlyEndDate { get; set; }

        [Display(Name = "Considera Exclusivas")]
        public bool? ConsidersExclusives { get; set; }
    }
}
