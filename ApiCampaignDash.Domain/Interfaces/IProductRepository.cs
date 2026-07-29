using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByCampaignIdAsync(int idCampaign);

        Task<(IEnumerable<Product> Items, int TotalCount)> GetByCampaignIdPagedAsync(int idCampaign, int? idProduct, int pageNumber, int pageSize);
    }
}
