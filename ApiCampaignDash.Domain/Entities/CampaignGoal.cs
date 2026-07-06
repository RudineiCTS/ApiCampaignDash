using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignGoal
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Required(ErrorMessage = "O campo Valor da Meta é obrigatório.")]
        [Display(Name = "Valor da Meta")]
        public decimal GoalValue { get; set; }

        [Display(Name = "Gatilho de Positivação")]
        public decimal PositivationTrigger { get; set; }

        [Display(Name = "Gatilho de Venda")]
        public decimal SalesTrigger { get; set; }
    }
}
