

using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface    ICampaignResumeSellOutService
    {
        Task<IEnumerable<CampaignResumeSellOutDto>> GetCampaignResumeSellOutsAsync(CampaignParamsDto campaign);
        Task<CampaignParamsDto?> GetCampaignParams(int idCampaign);
    }
}
