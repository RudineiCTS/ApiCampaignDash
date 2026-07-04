namespace ApiCampaignDash.Domain.Entities
{
    public class SalesOperatorBase
    {
        public int IdSupervisor { get; set; }
        public string SupervisorName { get; set; } = string.Empty;
        public int IdPersonSales { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public string OperatorUser { get; set; } = string.Empty;
        public int IdSector { get; set; }
    }
}
