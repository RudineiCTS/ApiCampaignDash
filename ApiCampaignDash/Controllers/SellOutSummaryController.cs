using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/sellout-summary")]
    [ApiController]
    public class SellOutSummaryController : ControllerBase
    {
        private readonly ISellOutSummaryService _service;

        public SellOutSummaryController(ISellOutSummaryService service)
        {
            _service = service;
        }

        // POST: api/sellout-summary
        [HttpPost]
        public async Task<ActionResult<SellOutSummaryDto>> GetTotals([FromBody] SellOutSummaryFilterDto filter)
        {
            var result = await _service.GetTotalsAsync(filter);
            return Ok(result);
        }

        // POST: api/sellout-summary/monthly
        [HttpPost("monthly")]
        public async Task<ActionResult<IEnumerable<SellOutMonthlySummaryDto>>> GetMonthlyTotals([FromBody] SellOutSummaryFilterDto filter)
        {
            var result = await _service.GetMonthlyTotalsAsync(filter);
            return Ok(result);
        }
    }
}
