using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IClientsRepository _repository;
        private readonly IMapper _mapper;

        public ClientsService(IClientsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientsDto>> GetByCampaignIdAsync(int idCampaign)
        {
            var clients = await _repository.GetByCampaignIdAsync(idCampaign);
            return _mapper.Map<IEnumerable<ClientsDto>>(clients);
        }
    }
}
