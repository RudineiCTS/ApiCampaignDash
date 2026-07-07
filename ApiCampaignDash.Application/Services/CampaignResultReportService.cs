using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class CampaignResultReportService : ICampaignResultReportService
    {
        private readonly ICampaignResultReportRepository _repository;
        private readonly IMapper _mapper;

        public CampaignResultReportService(ICampaignResultReportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CampaignResultReportDto>> GetResultsAsync(DateTime competenceDateFrom)
        {
            var results = await _repository.GetResultsAsync(competenceDateFrom);
            return _mapper.Map<IEnumerable<CampaignResultReportDto>>(results);
        }
    }
}
