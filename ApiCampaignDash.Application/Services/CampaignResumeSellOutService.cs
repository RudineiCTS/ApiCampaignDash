using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class CampaignResumeSellOutService : ICampaignResumeSellOutService
    {
        private readonly ICampaignResumeSellOutRepository _campaignResumeSellOutRepository;
        private readonly IClientsRepository _clientsRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IProductLineRepository _productLineRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CampaignResumeSellOutService(
            ICampaignResumeSellOutRepository campaignResumeSellOutRepository,
            IClientsRepository clientsRepository,
            IManufacturerRepository manufacturerRepository,
            IProductLineRepository productLineRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _campaignResumeSellOutRepository = campaignResumeSellOutRepository;
            _clientsRepository = clientsRepository;
            _manufacturerRepository = manufacturerRepository;
            _productLineRepository = productLineRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CampaignResumeSellOutDto>> GetCampaignResumeSellOutsAsync(CampaignParamsDto campaign)
        {
            var campaignParam = new CampaignParams
            {
                IdCampaign = campaign.IdCampaign,
                Manufacturer = _mapper.Map<List<Manufacturer>>(campaign.Manufacturer),
                ProductLines = _mapper.Map<List<ProductLine>>(campaign.ProductLines),
                Products = _mapper.Map<List<Product>>(campaign.Products),
                Clients = _mapper.Map<List<Clients>>(campaign.Clients),
                idOrigim = campaign.idOrigim,
                IsValidSellOutGC = campaign.IsValidSellOutGC,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate

            };
            var campaignResumeSellOuts = await _campaignResumeSellOutRepository.GetCampaignResumeSellOutsAsync(campaignParam);
            return (IEnumerable<CampaignResumeSellOutDto>)campaignResumeSellOuts;
        }

        public async Task<CampaignParamsDto> GetCampaignParams(int idCampaign, DateTime StartDate, DateTime EndDate)
        {
            var manufacturers = await _manufacturerRepository.GetByCampaignIdAsync(idCampaign);
            var manufacturerDtos = _mapper.Map<IEnumerable<ManufacturerDto>>(manufacturers);

            var productLines = await _productLineRepository.GetByCampaignIdAsync(idCampaign);
            var productLineDtos = _mapper.Map<IEnumerable<ProductLineDto>>(productLines);

            var products = await _productRepository.GetByCampaignIdAsync(idCampaign);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            var clients = await _clientsRepository.GetByCampaignIdAsync(idCampaign);
            var clientDtos = _mapper.Map<IEnumerable<ClientsDto>>(clients);


            return new CampaignParamsDto
            {
                Manufacturer = (List<ManufacturerDto>)manufacturerDtos,
                ProductLines = (List<ProductLineDto>)productLineDtos,
                Products = (List<ProductDto>)productDtos,
                Clients = (List<ClientsDto>)clientDtos,
                IdCampaign = idCampaign,
                StartDate = StartDate,
                EndDate = EndDate,
                /*fixos*/
                idOrigim=1,
                IsValidSellOutGC = true

            };
        }        
    }
}
