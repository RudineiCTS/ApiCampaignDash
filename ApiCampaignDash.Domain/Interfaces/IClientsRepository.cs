using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface IClientsRepository
    {
        Task<IEnumerable<Clients>> GetByCampaignIdAsync(int idCampaign);

        Task<(IEnumerable<Clients> Items, int TotalCount)> GetByCampaignIdPagedAsync(int idCampaign, int? idClient, int pageNumber, int pageSize);
    }
}
