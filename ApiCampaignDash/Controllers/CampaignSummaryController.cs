using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/campaign-summary")]
    [ApiController]
    public class CampaignSummaryController : ControllerBase
    {
        private readonly ICampaignSummaryService _service;

        public CampaignSummaryController(ICampaignSummaryService service)
        {
            _service = service;
        }

        // GET: api/campaign-summary?competenceDateFrom=2024-01-01
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignSummaryDto>>> GetSummary([FromQuery] DateTime competenceDateFrom)
        {
            var summary = await _service.GetSummaryAsync(competenceDateFrom);
            return Ok(summary);
        }
    }
}
