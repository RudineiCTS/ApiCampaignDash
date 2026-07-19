using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<Manufacturer, ManufacturerDto>().ReverseMap();
        }
    }
}
