using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IProductLineRepository
    {
        Task<IEnumerable<ProductLine>> GetByCampaignIdAsync(int idCampaign);
    }
}
