namespace ApiCampaignDash.Domain.Entities
{
    public class ValidationRule
    {
        public int IdCampaign { get; set; }
        public int IdCampaignToValidate { get; set; }
        public string? ValidationResult { get; set; }
    }
}
