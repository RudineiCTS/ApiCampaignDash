using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class SalesOperatorBase
    {
        [Key]
        [Display(Name = "Identificador do Supervisor")]
        public int IdSupervisor { get; set; }

        [Required(ErrorMessage = "O campo Nome do Supervisor é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Nome do Supervisor deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome do Supervisor")]
        public string SupervisorName { get; set; } = string.Empty;

        [Key]
        [Display(Name = "Identificador da Pessoa de Televendas")]
        public int IdPersonSales { get; set; }

        [Required(ErrorMessage = "O campo Nome do Digitador é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Nome do Digitador deve ter no máximo {1} caracteres.")]
        [Display(Name = "Nome do Digitador")]
        public string OperatorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Usuário do Digitador é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Usuário do Digitador deve ter no máximo {1} caracteres.")]
        [Display(Name = "Usuário do Digitador")]
        public string OperatorUser { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Identificador do Setor é obrigatório.")]
        [Display(Name = "Identificador do Setor")]
        public int IdSector { get; set; }
    }
}
