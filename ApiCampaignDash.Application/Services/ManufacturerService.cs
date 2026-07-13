using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _repository;
        private readonly IMapper _mapper;

        public ManufacturerService(IManufacturerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ManufacturerDto>> GetByCampaignIdAsync(int idCampaign)
        {
            var manufacturers = await _repository.GetByCampaignIdAsync(idCampaign);
            return _mapper.Map<IEnumerable<ManufacturerDto>>(manufacturers);
        }
    }
}
