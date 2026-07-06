using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetAll()
        {
            var campaigns = await _campaignService.GetAllAsync();
            return Ok(campaigns);
        }
        // GET: api/campaign/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CampaignDto>> GetById(int id)
        {
            var campaign = await _campaignService.GetByIdAsync(id);

            if (campaign == null)
                return NotFound();

            return Ok(campaign);
        }

        // GET: api/campaign/period?date=2026-07-06
        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetByPeriod([FromQuery] DateTime date)
        {
            var campaigns = await _campaignService.GetByPeriodCampaignAsync(date);
            return Ok(campaigns);
        }
    }
}
