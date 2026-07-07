using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class CampaignResultReportProfile : Profile
    {
        public CampaignResultReportProfile()
        {
            CreateMap<CampaignResultReport, CampaignResultReportDto>();
        }
    }
}
