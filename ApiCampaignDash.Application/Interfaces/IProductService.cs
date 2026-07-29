using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetByCampaignIdAsync(int idCampaign);

        Task<PagedResultDto<ProductDto>> GetByCampaignIdPagedAsync(int idCampaign, int? idProduct, int pageNumber, int pageSize);
    }
}
