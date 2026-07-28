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

        public async Task<PagedResultDto<ClientsDto>> GetByCampaignIdPagedAsync(int idCampaign, int? idClient, int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _repository.GetByCampaignIdPagedAsync(idCampaign, idClient, pageNumber, pageSize);

            return new PagedResultDto<ClientsDto>
            {
                Items = _mapper.Map<IEnumerable<ClientsDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
