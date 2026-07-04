namespace ApiCampaignDash.Domain.Entities
{
    public class IndividualGoal
    {
        public int IdCampaign { get; set; }
        public int IdPerson { get; set; }
        public int IdSupervisor { get; set; }
        public int PersonType { get; set; }
        public decimal GoalValue { get; set; }
        public decimal Trigger { get; set; }
        public int IdManager { get; set; }
    }
}
