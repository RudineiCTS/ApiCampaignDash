using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiCampaignDash.Controllers
{
    [Route("Campaign/Params/Detail")]
    [ApiController]
    public class CampaignParamsController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductLineService _productLineService;
        private readonly IProductService _productService;
        private readonly IClientsService _clientsService;

        public CampaignParamsController(
            IManufacturerService manufacturerService,
            IProductLineService productLineService,
            IProductService productService,
            IClientsService clientsService)
        {
            _manufacturerService = manufacturerService;
            _productLineService = productLineService;
            _productService = productService;
            _clientsService = clientsService;
        }

        // GET: Campaign/Params/Detail/fabricante/5
        [HttpGet("fabricante/{idCampaign:int}")]
        public async Task<ActionResult<IEnumerable<ManufacturerDto>>> GetManufacturers(int idCampaign)
        {
            var result = await _manufacturerService.GetByCampaignIdAsync(idCampaign);
            return Ok(result);
        }

        // GET: Campaign/Params/Detail/linha/5
        [HttpGet("linha/{idCampaign:int}")]
        public async Task<ActionResult<IEnumerable<ProductLineDto>>> GetProductLines(int idCampaign)
        {
            var result = await _productLineService.GetByCampaignIdAsync(idCampaign);
            return Ok(result);
        }

        // GET: Campaign/Params/Detail/produto/5
        [HttpGet("produto/{idCampaign:int}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(int idCampaign)
        {
            var result = await _productService.GetByCampaignIdAsync(idCampaign);
            return Ok(result);
        }

        // GET: Campaign/Params/Detail/cliente/5?idCliente=123&pageNumber=1&pageSize=50
        [HttpGet("cliente/{idCampaign:int}")]
        public async Task<ActionResult<PagedResultDto<ClientsDto>>> GetClients(
            int idCampaign,
            [FromQuery] int? idCliente,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;
            if (pageSize > 200) pageSize = 200;

            var result = await _clientsService.GetByCampaignIdPagedAsync(idCampaign, idCliente, pageNumber, pageSize);
            return Ok(result);
        }
    }
}
