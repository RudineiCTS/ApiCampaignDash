using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private sealed record ManufacturerRow(int IdCampaign, int IdManufacturer, string? ManufacturerName, string? IsValid);

        private readonly AppDbContext _context;

        public ManufacturerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Manufacturer>> GetByCampaignIdAsync(int idCampaign)
        {
            var rows = await _context.Database.SqlQuery<ManufacturerRow>($"""
                SELECT
                    tblCamTelFab.IDCampanhaTelevendas AS IdCampaign,
                    tblCamTelFab.IDFabricante AS IdManufacturer,
                    tblPesFisJur.Nome AS ManufacturerName,
                    tblCamTelFab.Contem AS IsValid
                FROM
                    GS300GP.dbo.tblCampanhaTelevendasFabricante tblCamTelFab (NOLOCK)
                LEFT JOIN
                    GS300ERP.dbo.uvwPessoaFisicaJuridica tblPesFisJur (NOLOCK) ON tblPesFisJur.IDPessoa = tblCamTelFab.IDFabricante
                WHERE
                    tblCamTelFab.IDCampanhaTelevendas = {idCampaign}
                """).ToListAsync();

            return rows.Select(r => new Manufacturer
            {
                IdCampaign = r.IdCampaign,
                IdManufacturer = r.IdManufacturer,
                Name = r.ManufacturerName ?? string.Empty,
                IsValid = r.IsValid
            });
        }
    }
}
