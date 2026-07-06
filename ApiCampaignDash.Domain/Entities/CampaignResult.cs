using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignResult
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Required(ErrorMessage = "O campo Data de Competência é obrigatório.")]
        [Display(Name = "Data de Competência")]
        public DateTime CompetenceDate { get; set; }

        [Required(ErrorMessage = "O campo Descrição da Campanha é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Descrição da Campanha deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição da Campanha")]
        public string CampaignDescription { get; set; } = string.Empty;

        [Key]
        [Display(Name = "Identificador da Pessoa de Televendas")]
        public int IdPersonSales { get; set; }

        [Required(ErrorMessage = "O campo Nome do Digitador é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Nome do Digitador deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome do Digitador")]
        public string OperatorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Ranking é obrigatório.")]
        [Display(Name = "Ranking")]
        public int Ranking { get; set; }

        [Required(ErrorMessage = "O campo Valor Apurado é obrigatório.")]
        [Display(Name = "Valor Apurado")]
        public decimal AssessedValue { get; set; }

        [Display(Name = "Premiação")]
        public decimal Award { get; set; }

        [Display(Name = "Log de Cálculo")]
        public string? CalculationLog { get; set; }

        [Required(ErrorMessage = "O campo Data de Cálculo é obrigatório.")]
        [Display(Name = "Data de Cálculo")]
        public DateTime CalculationDate { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Pessoa é obrigatório.")]
        [Display(Name = "Tipo de Pessoa")]
        public int PersonType { get; set; }

        [Required(ErrorMessage = "O campo Identificador do Supervisor é obrigatório.")]
        [Display(Name = "Identificador do Supervisor")]
        public int IdSupervisor { get; set; }

        [Required(ErrorMessage = "O campo Nome do Supervisor é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Nome do Supervisor deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome do Supervisor")]
        public string SupervisorName { get; set; } = string.Empty;

        [Display(Name = "Realizado Total")]
        public decimal TotalAchieved { get; set; }

        [Display(Name = "Objetivo Individual")]
        public decimal IndividualTarget { get; set; }

        [Display(Name = "Percentual Realizado Individual")]
        public decimal PercentageAchieved { get; set; }

        [Display(Name = "Valor Apurado Bees")]
        public decimal AssessedValueBees { get; set; }
    }
}
