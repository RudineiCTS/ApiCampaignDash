using ApiCampaignDash.Domain.Entities;

namespace ApiCampaignDash.Domain.Interfaces
{
    public interface ICampaignResumeSellOutRepository
    {
        Task<IEnumerable<CampaignResumeSellOut>> GetCampaignResumeSellOutsAsync(CampaignParams campaign);
    }
}
