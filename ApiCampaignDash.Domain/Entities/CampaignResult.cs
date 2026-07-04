namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignResult
    {
        public int IdCampaign { get; set; }
        public DateTime CompetenceDate { get; set; }
        public string CampaignDescription { get; set; } = string.Empty;
        public int IdPersonSales { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public int Ranking { get; set; }
        public decimal AssessedValue { get; set; }
        public decimal Award { get; set; }
        public string? CalculationLog { get; set; }
        public DateTime CalculationDate { get; set; }
        public int PersonType { get; set; }
        public int IdSupervisor { get; set; }
        public string SupervisorName { get; set; } = string.Empty;
        public decimal TotalAchieved { get; set; }
        public decimal IndividualTarget { get; set; }
        public decimal PercentageAchieved { get; set; }
        public decimal AssessedValueBees { get; set; }
    }
}
