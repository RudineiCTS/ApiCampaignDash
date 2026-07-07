using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class CampaignResultReportRepository : ICampaignResultReportRepository
    {
        private readonly AppDbContext _context;

        public CampaignResultReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CampaignResultReport>> GetResultsAsync(DateTime competenceDateFrom)
        {
            var results = await _context.Database.SqlQuery<CampaignResultReport>($"""
                SELECT
                    tblCamTelRes.IDCampanhaTelevendas AS IdCampaign,
                    tblCamTelRes.DescricaoCampanha AS CampaignDescription,
                    tblCamTelRes.DataCompetencia AS CompetenceDate,
                    CASE WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 1 THEN 'POSITIVAÇÃO'
                        WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 2 THEN 'VENDAS'
                        WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 3 THEN 'QUANTIDADE VENDIDA'
                        WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 6 THEN 'POSITIVAÇÃO ESPECÍFICA' END AS CampaignTypeDescription,
                    tblCamTelRes.IDSupervisor AS IdSupervisor,
                    tblCamTelRes.NomeSupervisor AS SupervisorName,
                    tblCamTelRes.IDPessoaTelevendas AS IdPersonSales,
                    tblCamTelRes.NomeDigitador AS OperatorName,
                    tblCamTelRes.ObjetivoIndividual AS IndividualTarget,
                    tblCamTelRes.ValorApurado AS AssessedValue,
                    tblCamTelRes.PercentualRealizadoIndividual AS PercentageAchieved,
                    tblCamTelRes.Ranking AS Ranking,
                    tblCamTelRes.Premiacao AS Award,
                    tblCamTelRes.LogCalculo AS CalculationLog,
                    tblCamTelRes.DataCalculo AS CalculationDate
                FROM
                    GS300GP.dbo.tblCampanhaTelevendas tblCamTel (NOLOCK)
                LEFT JOIN
                    GS300GP.dbo.tblCampanhaTelevendasResultado tblCamTelRes (NOLOCK)
                        ON tblCamTelRes.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
                        AND tblCamTelRes.DataCompetencia = tblCamTel.DataCompetencia
                WHERE
                    CAST(tblCamTel.DataCompetencia AS DATE) >= {competenceDateFrom} AND
                    tblCamTel.IDCampanhaTelevendasPeriodoCompetenciaSituacao <> 4
                """).ToListAsync();

            return results;
        }
    }
}
