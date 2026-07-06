using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class Combo
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador do Combo")]
        public int IdCombo { get; set; }

        [Display(Name = "Contém")]
        public string? IsValid { get; set; } = string.Empty;
    }
}
