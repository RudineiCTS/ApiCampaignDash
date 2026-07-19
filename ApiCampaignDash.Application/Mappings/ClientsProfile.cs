using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Domain.Entities;
using AutoMapper;

namespace ApiCampaignDash.Application.Mappings
{
    public class ClientsProfile : Profile
    {
        public ClientsProfile()
        {
            CreateMap<Clients, ClientsDto>().ReverseMap();
        }
    }
}
