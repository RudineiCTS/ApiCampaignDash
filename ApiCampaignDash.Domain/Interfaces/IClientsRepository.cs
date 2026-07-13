using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Clients>> GetByCampaignIdAsync(int idCampaign);
    }
}
