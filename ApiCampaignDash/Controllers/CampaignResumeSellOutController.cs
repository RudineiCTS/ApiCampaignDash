using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CampaignResumeSellOutController :ControllerBase
    {
        private readonly ICampaignResumeSellOutService _campaignResumeSellOutService;

        public CampaignResumeSellOutController(ICampaignResumeSellOutService campaignResumeSellOutService)
        {
            _campaignResumeSellOutService = campaignResumeSellOutService;
        }

        [HttpGet("campaign-resume-sellout/{id:int}")]
        public async Task<ActionResult<IEnumerable<CampaignResumeSellOutDto>>> GetCampaignResumeSellOuts(
               [FromRoute] int id,
               [FromQuery] DateTime startDate,
               [FromQuery] DateTime endDate
            )
        {
            var Campaignparams = await _campaignResumeSellOutService.GetCampaignParams(id, startDate, endDate);
            var campaignResumeSellOutsList = await _campaignResumeSellOutService.GetCampaignResumeSellOutsAsync(Campaignparams);
            return Ok(campaignResumeSellOutsList);
        }

    }
}
