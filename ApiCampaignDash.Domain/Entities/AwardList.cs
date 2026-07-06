using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class AwardList
    {
        [Key]
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Required(ErrorMessage = "O campo Valor Início é obrigatório.")]
        [Display(Name = "Valor Início")]
        public decimal StartValue { get; set; }

        [Required(ErrorMessage = "O campo Valor Fim é obrigatório.")]
        [Display(Name = "Valor Fim")]
        public decimal EndValue { get; set; }

        [Key]
        [Display(Name = "Ranking Início")]
        public int RankingStart { get; set; }

        [Required(ErrorMessage = "O campo Ranking Fim é obrigatório.")]
        [Display(Name = "Ranking Fim")]
        public int RankingEnd { get; set; }

        [Required(ErrorMessage = "O campo Valor do Prêmio é obrigatório.")]
        [Display(Name = "Valor do Prêmio")]
        public decimal AwardValue { get; set; }

        [Display(Name = "Limite do Prêmio")]
        public decimal AwardLimit { get; set; }

        [Key]
        [Display(Name = "Tipo de Pessoa")]
        public int PersonType { get; set; }
    }
}
