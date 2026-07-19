using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class CampaignResumeSellOutProfile:Profile
    {
        public CampaignResumeSellOutProfile()
        {
            CreateMap<CampaignResumeSellOut, CampaignResumeSellOutDto>();            
        }
    }
}
