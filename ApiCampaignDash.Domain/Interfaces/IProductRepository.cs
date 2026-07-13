using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByCampaignIdAsync(int idCampaign);
    }
}
