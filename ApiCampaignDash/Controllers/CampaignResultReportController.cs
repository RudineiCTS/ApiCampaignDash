using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/campaign-result-report")]
    [ApiController]
    public class CampaignResultReportController : ControllerBase
    {
        private readonly ICampaignResultReportService _service;

        public CampaignResultReportController(ICampaignResultReportService service)
        {
            _service = service;
        }

        // GET: api/campaign-result-report?competenceDateFrom=2023-06-30
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignResultReportDto>>> GetResults([FromQuery] DateTime competenceDateFrom)
        {
            var results = await _service.GetResultsAsync(competenceDateFrom);
            return Ok(results);
        }
    }
}
