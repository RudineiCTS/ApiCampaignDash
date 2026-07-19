using System.ComponentModel.DataAnnotations;

namespace ApiCampaignDash.Application.DTOs
{
    public class CampaignResumeSellOutDto
    {
        public int IdCampaign { get; set; }        
        public string CPFCNPJ { get; set; } = string.Empty;        
        public string LegalName { get; set; } = string.Empty;        
        public string ProductCode { get; set; } = string.Empty;        
        public string ProductName { get; set; } = string.Empty;        
        public string ProductEan { get; set; } = string.Empty;        
        public int QuantitySold { get; set; }       
        public decimal ValueSold { get; set; }
        public string SellerName { get; set; } = string.Empty;
    }
}
