using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class ManufacturerPersonDistinction
    {
        [Key]
        [Display(Name = "Identificador do Fabricante")]
        public int IdManufacturer { get; set; }

        [Required(ErrorMessage = "O campo Descrição do Fabricante é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo Descrição do Fabricante deve ter no máximo {1} caracteres.")]
        [Display(Name = "Descrição do Fabricante")]
        public string ManufacturerDescription { get; set; } = string.Empty;

        [Key]
        [Display(Name = "Identificador da Pessoa")]
        public int IdPerson { get; set; }

        [Required(ErrorMessage = "O campo Identificador do Digitador é obrigatório.")]
        [Display(Name = "Identificador do Digitador")]
        public int IdOperator { get; set; }
    }
}
