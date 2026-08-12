using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class SellOutSummaryProfile : Profile
    {
        public SellOutSummaryProfile()
        {
            CreateMap<SellOutSummaryFilterDto, SellOutSummaryFilter>();
            CreateMap<SellOutSummary, SellOutSummaryDto>();
            CreateMap<SellOutMonthlySummary, SellOutMonthlySummaryDto>();
        }
    }
}
