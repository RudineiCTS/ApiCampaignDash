using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/dynamic-report")]
    [ApiController]
    public class DynamicReportController : ControllerBase
    {
        private readonly IDynamicReportService _service;

        public DynamicReportController(IDynamicReportService service)
        {
            _service = service;
        }

        // POST: api/dynamic-report
        [HttpPost]
        public async Task<ActionResult<DynamicReportResponseDto>> GetReport([FromBody] DynamicReportRequestDto request)
        {
            try
            {
                var result = await _service.GetReportAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
