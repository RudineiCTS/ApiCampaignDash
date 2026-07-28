namespace ApiCampaignDash.Application.DTOs
{
    public class CampaignSummaryDto
    {
        public int IdCampaign { get; set; }
        public string? CampaignDescription { get; set; }
        public DateTime? CompetenceDate { get; set; }
        public string? CampaignTypeDescription { get; set; }
        public decimal? GoalValue { get; set; }
        public decimal? AssessedValue { get; set; }
        public decimal? AssessedValueBees { get; set; }
        public decimal? TotalAward { get; set; }
        public decimal? TotalPot { get; set; }
        public decimal? PercentageAchieved { get; set; }
        public string? Notes { get; set; }
        public string? TypeCampaign { get; set; }
        public bool? IsDynamic { get; set; }
        public int IdCompetencePeriodStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalRanking { get; set; }
        public int IdAssessmentType { get; set; }
        public int IdCalculationMethod { get; set; }
        public int? ValidationRule { get; set; }
        public int ValueType { get; set; }
        public DateTime? EarlyEndDate { get; set; }
        public bool? ConsidersExclusives { get; set; }
    }
}
