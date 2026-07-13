using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class ProductLineProfile : Profile
    {
        public ProductLineProfile()
        {
            CreateMap<ProductLine, ProductLineDto>();
        }
    }
}
