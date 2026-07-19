using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
