using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetByCampaignIdAsync(int idCampaign);
    }
}
