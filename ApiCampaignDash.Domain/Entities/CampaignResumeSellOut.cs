using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Domain.Entities
{
    public class CampaignResumeSellOut
    {
        [Display(Name = "Identificador da Campanha")]
        public int IdCampaign { get; set; }

        [Display(Name ="Cliente")]
        public string CPFCNPJ { get; set; } = string.Empty;

        [Display(Name = "Razão Social")]
        public string LegalName { get; set; } = string.Empty;

        [Display(Name = "Codigo do Produto")]
        public string ProductCode { get; set; } = string.Empty;

        [Display(Name = "Descrição do Produto")]
        public string ProductName { get; set; } = string.Empty;

        [Display(Name ="EAN")]
        public string ProductEan { get; set; }= string.Empty;

        [Display(Name = "Quantidade Vendida")]
        public int QuantitySold { get; set; }

        [Display(Name = "Valor Vendido")]
        public decimal ValueSold { get; set; }

        [Display(Name = "Nome Vendedor")]
        public string SellerName { get; set; } = string.Empty;

    }
}
