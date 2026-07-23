using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/campaign-summary-by-description")]
    [ApiController]
    public class CampaignSummaryByDescriptionController : ControllerBase
    {
        private readonly ICampaignSummaryByDescriptionService _service;

        public CampaignSummaryByDescriptionController(ICampaignSummaryByDescriptionService service)
        {
            _service = service;
        }

        // GET: api/campaign-summary-by-description?competenceDateFrom=2026-01-31&description=Ache
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignSummaryDto>>> GetSummaryByDescription(
            [FromQuery] DateTime competenceDateFrom,
            [FromQuery] string description)
        {
            var summary = await _service.GetSummaryByDescriptionAsync(competenceDateFrom, description);
            return Ok(summary);
        }
    }
}
