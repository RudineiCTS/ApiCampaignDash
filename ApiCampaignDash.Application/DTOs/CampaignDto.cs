namespace ApiCampaignDash.Application.DTOs
{
    public class CampaignDto
    {

        public int IdCampaign { get; set; }


        public DateTime? CompetenceDate { get; set; }


        public int IdCompetencePeriodStatus { get; set; }


        public DateTime? StartDate { get; set; }


        public DateTime? EndDate { get; set; }

        public int? TotalRanking { get; set; }


        public string Description { get; set; } = string.Empty;


        public int IdAssessmentType { get; set; }


        public int IdCalculationMethod { get; set; }


        public int? ValidationRule { get; set; }

        public int ValueType { get; set; }


        public DateTime? EarlyEndDate { get; set; }

        public string? Notes { get; set; }

        public bool? ConsidersExclusives { get; set; }
        public bool? CampaignType { get; set; }
    }
}
