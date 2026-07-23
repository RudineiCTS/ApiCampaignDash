using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class CampaignSummaryByDescriptionService : ICampaignSummaryByDescriptionService
    {
        private readonly ICampaignSummaryByDescriptionRepository _repository;
        private readonly IMapper _mapper;

        public CampaignSummaryByDescriptionService(ICampaignSummaryByDescriptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CampaignSummaryDto>> GetSummaryByDescriptionAsync(DateTime competenceDateFrom, string description)
        {
            var summaries = await _repository.GetSummaryByDescriptionAsync(competenceDateFrom, description);
            return _mapper.Map<IEnumerable<CampaignSummaryDto>>(summaries);
        }
    }
}
