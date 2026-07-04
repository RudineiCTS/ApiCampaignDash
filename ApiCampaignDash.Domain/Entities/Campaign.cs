namespace ApiCampaignDash.Domain.Entities
{
    public class Campaign
    {
        public int IdCampaign { get; set; }
        public DateTime CompetenceDate { get; set; }
        public int IdCompetencePeriodStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalRanking { get; set; }
        public string Description { get; set; } = string.Empty;
        public int IdAssessmentType { get; set; }
        public int IdCalculationMethod { get; set; }
        public string? ValidationRule { get; set; }
        public string? ValueType { get; set; }
        public DateTime? EarlyEndDate { get; set; }
        public string? Notes { get; set; }
        public bool ConsidersExclusives { get; set; }
        public int CampaignType { get; set; }
    }
}
