using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class ValidationRule
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador da Campanha a Validar")]
        public int IdCampaignToValidate { get; set; }

        [Display(Name = "Resultado da Validação")]
        public string? ValidationResult { get; set; }
    }
}
