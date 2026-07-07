using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class CampaignSummaryProfile : Profile
    {
        public CampaignSummaryProfile()
        {
            CreateMap<CampaignSummary, CampaignSummaryDto>();
        }
    }
}
