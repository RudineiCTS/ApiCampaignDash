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
    }
}
