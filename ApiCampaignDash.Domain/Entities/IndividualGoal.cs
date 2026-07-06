using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class IndividualGoal
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Key]
        [Display(Name = "Identificador da Pessoa")]
        public int IdPerson { get; set; }

        [Required(ErrorMessage = "O campo Identificador do Supervisor é obrigatório.")]
        [Display(Name = "Identificador do Supervisor")]
        public int IdSupervisor { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Pessoa é obrigatório.")]
        [Display(Name = "Tipo de Pessoa")]
        public int PersonType { get; set; }

        [Required(ErrorMessage = "O campo Valor da Meta é obrigatório.")]
        [Display(Name = "Valor da Meta")]
        public decimal GoalValue { get; set; }

        [Display(Name = "Gatilho")]
        public decimal Trigger { get; set; }

        [Display(Name = "Identificador do Gerente")]
        public int IdManager { get; set; }
    }
}
