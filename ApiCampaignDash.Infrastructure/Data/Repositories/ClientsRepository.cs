using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private sealed record ClientsRow(int IdCampaign, int IdClients, string? ClientName, string? CpfCnpj, string? City, string? State, string? IsValid);

        private readonly AppDbContext _context;

        public ClientsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Clients>> GetByCampaignIdAsync(int idCampaign)
        {
            var rows = await _context.Database.SqlQuery<ClientsRow>($"""
                SELECT
                    tblCamTelCli.IDCampanhaTelevendas AS IdCampaign,
                    tblCamTelCli.IDCliente AS IdClients,
                    tblCli.NomeCliente AS ClientName,
                    tblCli.CPF_CNPJ AS CpfCnpj,
                    tblCli.DescCidade AS City,
                    tblCli.UFCidade AS State,
                    tblCamTelCli.Contem AS IsValid
                FROM
                    GS300GP.dbo.tblCampanhaTelevendasCliente tblCamTelCli (NOLOCK)
                LEFT JOIN
                    GS300ERP.dbo.uvwClienteOutrasInformacoes tblCli (NOLOCK) ON tblCli.IDCliente = tblCamTelCli.IDCliente
                WHERE
                    tblCamTelCli.IDCampanhaTelevendas = {idCampaign}
                """).ToListAsync();

            return rows.Select(r => new Clients
            {
                IdCampaign = r.IdCampaign,
                IdClients = r.IdClients,
                ClientName = r.ClientName ?? string.Empty,
                CpfCnpj = r.CpfCnpj,
                City = r.City,
                State = r.State,
                IsValid = r.IsValid
            });
        }

        public async Task<(IEnumerable<Clients> Items, int TotalCount)> GetByCampaignIdPagedAsync(int idCampaign, int? idClient, int pageNumber, int pageSize)
        {
            var offset = (pageNumber - 1) * pageSize;

            var totalCount = await _context.Database.SqlQuery<int>($"""
                SELECT
                    COUNT(*) AS Value
                FROM
                    GS300GP.dbo.tblCampanhaTelevendasCliente tblCamTelCli (NOLOCK)
                WHERE
                    tblCamTelCli.IDCampanhaTelevendas = {idCampaign}
                    AND ({idClient} IS NULL OR tblCamTelCli.IDCliente = {idClient})
                """).SingleAsync();

            var rows = await _context.Database.SqlQuery<ClientsRow>($"""
                SELECT
                    tblCamTelCli.IDCampanhaTelevendas AS IdCampaign,
                    tblCamTelCli.IDCliente AS IdClients,
                    tblCli.NomeCliente AS ClientName,
                    tblCli.CPF_CNPJ AS CpfCnpj,
                    tblCli.DescCidade AS City,
                    tblCli.UFCidade AS State,
                    tblCamTelCli.Contem AS IsValid
                FROM
                    GS300GP.dbo.tblCampanhaTelevendasCliente tblCamTelCli (NOLOCK)
                LEFT JOIN
                    GS300ERP.dbo.uvwClienteOutrasInformacoes tblCli (NOLOCK) ON tblCli.IDCliente = tblCamTelCli.IDCliente
                WHERE
                    tblCamTelCli.IDCampanhaTelevendas = {idCampaign}
                    AND ({idClient} IS NULL OR tblCamTelCli.IDCliente = {idClient})
                ORDER BY
                    tblCamTelCli.IDCliente
                OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY
                """).ToListAsync();

            var items = rows.Select(r => new Clients
            {
                IdCampaign = r.IdCampaign,
                IdClients = r.IdClients,
                ClientName = r.ClientName ?? string.Empty,
                CpfCnpj = r.CpfCnpj,
                City = r.City,
                State = r.State,
                IsValid = r.IsValid
            });

            return (items, totalCount);
        }
    }
}
