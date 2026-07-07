using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignResultReport
    {
        [Display(Name = "Identificador da Campanha")]
        public int? IdCampaign { get; set; }

        [StringLength(200)]
        [Display(Name = "Descrição da Campanha")]
        public string? CampaignDescription { get; set; }

        [Display(Name = "Data de Competência")]
        public DateTime? CompetenceDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Tipo de Campanha")]
        public string? CampaignTypeDescription { get; set; }

        [Display(Name = "Identificador do Supervisor")]
        public int? IdSupervisor { get; set; }

        [StringLength(200)]
        [Display(Name = "Nome do Supervisor")]
        public string? SupervisorName { get; set; }

        [Display(Name = "Identificador da Pessoa de Televendas")]
        public int? IdPersonSales { get; set; }

        [StringLength(200)]
        [Display(Name = "Nome do Digitador")]
        public string? OperatorName { get; set; }

        [Display(Name = "Objetivo Individual")]
        public decimal? IndividualTarget { get; set; }

        [Display(Name = "Valor Apurado")]
        public decimal? AssessedValue { get; set; }

        [Display(Name = "Percentual Realizado")]
        public decimal? PercentageAchieved { get; set; }

        [Display(Name = "Ranking")]
        public int? Ranking { get; set; }

        [Display(Name = "Premiação")]
        public decimal? Award { get; set; }

        [Display(Name = "Log de Cálculo")]
        public string? CalculationLog { get; set; }

        [Display(Name = "Data de Cálculo")]
        public DateTime? CalculationDate { get; set; }
    }
}
