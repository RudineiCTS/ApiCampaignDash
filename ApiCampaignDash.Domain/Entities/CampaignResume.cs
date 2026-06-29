namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignResume
    {
        public int IdCampaign { get; set; } 
        public string TitleCampaign { get; set; }
        public string CampaignType { get; set; }
        public decimal SalesTarget { get; set; }
        public decimal AwardValue { get; set; }
        public decimal ValueRealized { get; set; }
        public int VerificationType { get; set; }
        public int CalculationType { get; set; }
        public bool isValidExclusive { get; set; }
    }
}
