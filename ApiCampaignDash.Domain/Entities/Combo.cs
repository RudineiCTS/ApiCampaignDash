namespace ApiCampaignDash.Domain.Entities
{
    public class Combo
    {
        public int IdCampaign { get; set; }
        public int IdCombo { get; set; }
        public string? IsValid { get; set; } = string.Empty;
    }
}
