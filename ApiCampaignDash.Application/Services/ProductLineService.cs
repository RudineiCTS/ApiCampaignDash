using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class ProductLineService : IProductLineService
    {
        private readonly IProductLineRepository _repository;
        private readonly IMapper _mapper;

        public ProductLineService(IProductLineRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductLineDto>> GetByCampaignIdAsync(int idCampaign)
        {
            var productLines = await _repository.GetByCampaignIdAsync(idCampaign);
            return _mapper.Map<IEnumerable<ProductLineDto>>(productLines);
        }
    }
}
