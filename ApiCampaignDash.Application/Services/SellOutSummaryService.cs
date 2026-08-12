using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class SellOutSummaryService : ISellOutSummaryService
    {
        private readonly ISellOutSummaryRepository _repository;
        private readonly IMapper _mapper;

        public SellOutSummaryService(ISellOutSummaryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SellOutSummaryDto> GetTotalsAsync(SellOutSummaryFilterDto filter)
        {
            var entityFilter = _mapper.Map<Domain.Entities.SellOutSummaryFilter>(filter);
            var result = await _repository.GetTotalsAsync(entityFilter);
            return _mapper.Map<SellOutSummaryDto>(result);
        }

        public async Task<IEnumerable<SellOutMonthlySummaryDto>> GetMonthlyTotalsAsync(SellOutSummaryFilterDto filter)
        {
            var entityFilter = _mapper.Map<Domain.Entities.SellOutSummaryFilter>(filter);
            var result = await _repository.GetMonthlyTotalsAsync(entityFilter);
            return _mapper.Map<IEnumerable<SellOutMonthlySummaryDto>>(result);
        }
    }
}
