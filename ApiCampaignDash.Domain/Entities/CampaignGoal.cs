namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignGoal
    {
        public int IdCampaign { get; set; }
        public decimal GoalValue { get; set; }
        public decimal PositivationTrigger { get; set; }
        public decimal SalesTrigger { get; set; }
    }
}
