using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface IManufacturerService
    {
        Task<IEnumerable<ManufacturerDto>> GetByCampaignIdAsync(int idCampaign);
    }
}
