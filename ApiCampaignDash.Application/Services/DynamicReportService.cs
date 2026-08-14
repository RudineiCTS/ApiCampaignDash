using ApiCampaignDash.Application.DTOs;
using ApiCampaignDash.Application.Interfaces;
using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;

namespace ApiCampaignDash.Application.Services
{
    public class DynamicReportService : IDynamicReportService
    {
        private static readonly HashSet<string> ValidDimensionFields = new()
        {
            "IDCampanha", "NomeCampanha", "IDFabricante", "NomeFabricante",
            "Linha", "Origem", "IDVendedor", "IDSupervisor", "IDGerente", "DataCompetencia"
        };

        private static readonly HashSet<string> ValidMetricFields = new() { "ValorVendido", "Positivacao" };

        private readonly IDynamicReportRepository _repository;
        private readonly ICampaignResumeSellOutService _campaignResumeSellOutService;
        private readonly ICampaignRepository _campaignRepository;

        public DynamicReportService(
            IDynamicReportRepository repository,
            ICampaignResumeSellOutService campaignResumeSellOutService,
            ICampaignRepository campaignRepository)
        {
            _repository = repository;
            _campaignResumeSellOutService = campaignResumeSellOutService;
            _campaignRepository = campaignRepository;
        }

        public async Task<DynamicReportResponseDto> GetReportAsync(DynamicReportRequestDto request)
        {
            var metrics = (request.Metrics ?? new()).Distinct().ToList();
            if (metrics.Count == 0)
                throw new ArgumentException("Selecione ao menos uma métrica para gerar o relatório.");

            foreach (var metric in metrics)
                if (!ValidMetricFields.Contains(metric))
                    throw new ArgumentException($"Métrica desconhecida: {metric}");

            var groupBy = (request.GroupBy ?? new())
                .Concat(request.Columns ?? new())
                .Distinct()
                .ToList();

            foreach (var field in groupBy)
                if (!ValidDimensionFields.Contains(field))
                    throw new ArgumentException($"Campo de agrupamento desconhecido: {field}");

            var filters = (request.Filters ?? new())
                .Where(f => f.Values != null && f.Values.Any(v => !string.IsNullOrWhiteSpace(v)))
                .Select(f => new DynamicReportFilter
                {
                    Field = f.Field,
                    Values = f.Values.Where(v => !string.IsNullOrWhiteSpace(v)).ToList()
                })
                .ToList();

            foreach (var filter in filters)
                if (!ValidDimensionFields.Contains(filter.Field))
                    throw new ArgumentException($"Campo de filtro desconhecido: {filter.Field}");

            var query = new DynamicReportQuery
            {
                GroupBy = groupBy,
                Metrics = metrics,
                Filters = filters,
                ManufacturerIds = (request.Scope?.IdManufacturers ?? new()).Distinct().ToList(),
                ProductLineIds = ParseIds(request.Scope?.ProductLines),
                CompetenceMonths = (request.Scope?.CompetenceMonths ?? new())
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct()
                    .ToList(),
            };

            var idCampaigns = (request.Scope?.IdCampaigns ?? new()).Distinct().ToList();
            if (idCampaigns.Count > 1)
                throw new ArgumentException("Selecione no máximo uma campanha no escopo da consulta.");

            if (idCampaigns.Count == 1)
                await ApplyCampaignScopeAsync(query, idCampaigns[0]);

            var result = await _repository.GetReportAsync(query);

            return new DynamicReportResponseDto
            {
                Rows = result.Rows
                    .Select(r => new DynamicReportRowDto { Dimensions = r.Dimensions, Metrics = r.Metrics })
                    .ToList(),
                Totals = result.Totals,
                GroupCount = result.GroupCount,
            };
        }

        private async Task ApplyCampaignScopeAsync(DynamicReportQuery query, int idCampaign)
        {
            var campaignParams = await _campaignResumeSellOutService.GetCampaignParams(idCampaign);
            if (campaignParams == null)
                throw new ArgumentException($"Campanha {idCampaign} não encontrada.");

            var campaign = await _campaignRepository.GetByIdAsync(idCampaign);

            query.IdCampaign = idCampaign;
            query.CampaignName = campaign?.Description;
            query.StartDate = campaignParams.StartDate;
            query.EndDate = campaignParams.EndDate;

            query.ManufacturerIds = query.ManufacturerIds
                .Concat(campaignParams.Manufacturer.Select(m => m.IdManufacturer))
                .Distinct()
                .ToList();

            query.ProductLineIds = query.ProductLineIds
                .Concat(campaignParams.ProductLines.Select(p => p.IdProductLine))
                .Distinct()
                .ToList();

            query.ProductIds = campaignParams.Products.Select(p => p.IdProduct).Distinct().ToList();
            query.ClientIds = campaignParams.Clients.Select(c => c.IdClients).Distinct().ToList();
        }

        private static List<int> ParseIds(List<string>? values)
        {
            if (values == null) return new();
            return values
                .Select(v => int.TryParse(v, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();
        }
    }
}
