using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface IClientsService
    {
        Task<IEnumerable<ClientsDto>> GetByCampaignIdAsync(int idCampaign);

        Task<PagedResultDto<ClientsDto>> GetByCampaignIdPagedAsync(int idCampaign, int? idClient, int pageNumber, int pageSize);
    }
}
