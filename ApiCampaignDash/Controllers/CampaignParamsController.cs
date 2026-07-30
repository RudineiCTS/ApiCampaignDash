using System.Text;
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

        // GET: Campaign/Params/Detail/produto/5?idProduto=701&pageNumber=1&pageSize=50
        [HttpGet("produto/{idCampaign:int}")]
        public async Task<ActionResult<PagedResultDto<ProductDto>>> GetProducts(
            int idCampaign,
            [FromQuery] int? idProduto,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;
            if (pageSize > 200) pageSize = 200;

            var result = await _productService.GetByCampaignIdPagedAsync(idCampaign, idProduto, pageNumber, pageSize);
            return Ok(result);
        }

        // GET: Campaign/Params/Detail/produto/5/todos
        [HttpGet("produto/{idCampaign:int}/todos")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(int idCampaign)
        {
            var result = await _productService.GetByCampaignIdAsync(idCampaign);
            return Ok(result);
        }

        // GET: Campaign/Params/Detail/produto/5/arquivo
        [HttpGet("produto/{idCampaign:int}/arquivo")]
        public async Task<IActionResult> GetProductsFile(int idCampaign)
        {
            var products = await _productService.GetByCampaignIdAsync(idCampaign);
            var csvBytes = BuildProductsCsv(products);

            return File(csvBytes, "text/csv", $"produtos_campanha_{idCampaign}.csv");
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

        // GET: Campaign/Params/Detail/cliente/5/todos
        [HttpGet("cliente/{idCampaign:int}/todos")]
        public async Task<ActionResult<IEnumerable<ClientsDto>>> GetAllClients(int idCampaign)
        {
            var result = await _clientsService.GetByCampaignIdAsync(idCampaign);
            return Ok(result);
        }

        // GET: Campaign/Params/Detail/cliente/5/arquivo
        [HttpGet("cliente/{idCampaign:int}/arquivo")]
        public async Task<IActionResult> GetClientsFile(int idCampaign)
        {
            var clients = await _clientsService.GetByCampaignIdAsync(idCampaign);
            var csvBytes = BuildClientsCsv(clients);

            return File(csvBytes, "text/csv", $"clientes_campanha_{idCampaign}.csv");
        }

        private static byte[] BuildClientsCsv(IEnumerable<ClientsDto> clients)
        {
            var builder = new StringBuilder();
            builder.AppendLine("IdCampaign;IdClients;ClientName;CpfCnpj;City;State;IsValid");

            foreach (var client in clients)
            {
                builder.AppendLine(string.Join(";",
                    client.IdCampaign,
                    client.IdClients,
                    EscapeCsvField(client.ClientName),
                    EscapeCsvField(client.CpfCnpj),
                    EscapeCsvField(client.City),
                    EscapeCsvField(client.State),
                    EscapeCsvField(client.IsValid)));
            }

            var preamble = Encoding.UTF8.GetPreamble();
            var content = Encoding.UTF8.GetBytes(builder.ToString());
            return [.. preamble, .. content];
        }

        private static byte[] BuildProductsCsv(IEnumerable<ProductDto> products)
        {
            var builder = new StringBuilder();
            builder.AppendLine("IdCampaign;IdProduct;Name;IsValid");

            foreach (var product in products)
            {
                builder.AppendLine(string.Join(";",
                    product.IdCampaign,
                    product.IdProduct,
                    EscapeCsvField(product.Name),
                    EscapeCsvField(product.IsValid)));
            }

            var preamble = Encoding.UTF8.GetPreamble();
            var content = Encoding.UTF8.GetBytes(builder.ToString());
            return [.. preamble, .. content];
        }

        private static string EscapeCsvField(string? field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            if (field.Contains(';') || field.Contains('"') || field.Contains('\n'))
                return $"\"{field.Replace("\"", "\"\"")}\"";

            return field;
        }
    }
}
