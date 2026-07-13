using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface IProductLineService
    {
        Task<IEnumerable<ProductLineDto>> GetByCampaignIdAsync(int idCampaign);
    }
}
