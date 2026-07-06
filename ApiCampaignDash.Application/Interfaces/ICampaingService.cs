using ApiCampaignDash.Application.DTOs;

namespace ApiCampaignDash.Application.Interfaces
{
    public interface ICampaignService
    {
        Task<IEnumerable<CampaignDto>> GetAllAsync();
        Task<CampaignDto?> GetByIdAsync(int id);
        Task<IEnumerable<CampaignDto>> GetByPeriodCampaignAsync(DateTime datetime);
    }
}
