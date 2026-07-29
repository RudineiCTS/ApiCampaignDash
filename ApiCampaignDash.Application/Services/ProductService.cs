using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Interfaces;
using AutoMapper;

namespace ApiCampaignDash.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetByCampaignIdAsync(int idCampaign)
        {
            var products = await _repository.GetByCampaignIdAsync(idCampaign);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<PagedResultDto<ProductDto>> GetByCampaignIdPagedAsync(int idCampaign, int? idProduct, int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _repository.GetByCampaignIdPagedAsync(idCampaign, idProduct, pageNumber, pageSize);

            return new PagedResultDto<ProductDto>
            {
                Items = _mapper.Map<IEnumerable<ProductDto>>(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
