using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _repository;
        private readonly IMapper _mapper;


        public CampaignService(ICampaignRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CampaignDto>> GetAllAsync()
        {
            var campaigns = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }

        public async Task<CampaignDto?> GetByIdAsync(int id)
        {
            var campaign = await _repository.GetByIdAsync(id);
            return campaign == null ? null : _mapper.Map<CampaignDto>(campaign);
        }

        public async Task<IEnumerable<CampaignDto>> GetByPeriodCampaignAsync(DateTime datetime)
        {
            var campaigns = await _repository.GetByPeriodCampaignAsync(datetime);
            return _mapper.Map<IEnumerable<CampaignDto>>(campaigns);
        }
    }
}
