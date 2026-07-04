namespace ApiCampaignDash.Domain.Entities
{
    public class AwardList
    {
        public int IdCampaign { get; set; }
        public decimal StartValue { get; set; }
        public decimal EndValue { get; set; }
        public int RankingStart { get; set; }
        public int RankingEnd { get; set; }
        public decimal AwardValue { get; set; }
        public decimal AwardLimit { get; set; }
        public int PersonType { get; set; }
    }
}
