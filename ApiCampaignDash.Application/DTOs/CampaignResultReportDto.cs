namespace ApiCampaignDash.Application.DTOs
{
    public class CampaignResultReportDto
    {
        public int? IdCampaign { get; set; }
        public string? CampaignDescription { get; set; }
        public DateTime? CompetenceDate { get; set; }
        public string? CampaignTypeDescription { get; set; }
        public int? IdSupervisor { get; set; }
        public string? SupervisorName { get; set; }
        public int? IdPersonSales { get; set; }
        public string? OperatorName { get; set; }
        public decimal? IndividualTarget { get; set; }
        public decimal? AssessedValue { get; set; }
        public decimal? PercentageAchieved { get; set; }
        public int? Ranking { get; set; }
        public decimal? Award { get; set; }
        public string? CalculationLog { get; set; }
        public DateTime? CalculationDate { get; set; }
    }
}
