using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class CampaignSummaryService : ICampaignSummaryService
    {
        private readonly ICampaignSummaryRepository _repository;
        private readonly IMapper _mapper;

        public CampaignSummaryService(ICampaignSummaryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CampaignSummaryDto>> GetSummaryAsync(DateTime competenceDateFrom)
        {
            var summaries = await _repository.GetSummaryAsync(competenceDateFrom);
            return _mapper.Map<IEnumerable<CampaignSummaryDto>>(summaries);
        }

        public async Task<IEnumerable<CampaignResultReportDto>> GetDetailsAsync(int idCampaign)
        {
            var details = await _repository.GetDetailsAsync(idCampaign);
            return _mapper.Map<IEnumerable<CampaignResultReportDto>>(details);
        }
    }
}
