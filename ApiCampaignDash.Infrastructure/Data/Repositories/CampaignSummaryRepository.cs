using ApiCampaignDash.Domain.Entities;
using ApiCampaignDash.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data.Repositories
{
    public class CampaignSummaryRepository : ICampaignSummaryRepository
    {
        private const string Sql = """
            SELECT
                tblCamTel.IDCampanhaTelevendas AS IdCampaign,
                tblCamTelRes.DescricaoCampanha AS CampaignDescription,
                tblCamTelRes.DataCompetencia AS CompetenceDate,
                CASE WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 1 THEN 'POSITIVAÇÃO'
                    WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 2 THEN 'VENDAS'
                    WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 3 THEN 'QUANTIDADE VENDIDA'
                    WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 6 THEN 'POSITIVAÇÃO ESPECÍFICA' END AS CampaignTypeDescription,
                tblCamTelMet.MetaValor AS GoalValue,
                tblCamTelRes.ValorApurado AS AssessedValue,
                tblCamTelRes.ValorApuradoBees AS AssessedValueBees,
                tblPre.PremiacaoTotal AS TotalAward,
                SUM(tblCamTelFaiPre.PremioValor) AS TotalPot,
                CASE WHEN tblCamTelMet.MetaValor = 0 THEN 0 ELSE CAST((tblCamTelRes.ValorApurado / tblCamTelMet.MetaValor) * 100 AS DECIMAL(18,2)) END AS PercentageAchieved,
                tblCamTel.Observacao AS Notes
            FROM
                GS300GP.dbo.tblCampanhaTelevendas tblCamTel (NOLOCK)
            LEFT JOIN
                GS300GP.dbo.tblCampanhaTelevendasResultado tblCamTelRes (NOLOCK) ON tblCamTelRes.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
            LEFT JOIN
                GS300GP.dbo.tblCampanhaTelevendasMeta tblCamTelMet (NOLOCK) ON tblCamTelMet.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
            LEFT JOIN
                (
                SELECT
                    tblCamTelRes.IDCampanhaTelevendas,
                    tblCamTelRes.DescricaoCampanha,
                    CASE WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 1 THEN 'POSITIVAÇÃO'
                        WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 2 THEN 'VENDAS'
                        WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 3 THEN 'QUANTIDADE VENDIDA'
                        WHEN tblCamTel.IDCampanhaTelevendasTipoApuracao = 6 THEN 'POSITIVAÇÃO ESPECÍFICA' END [TipoCampanha],
                    SUM(tblCamTelRes.Premiacao) PremiacaoTotal
                FROM
                    GS300GP.dbo.tblCampanhaTelevendas tblCamTel (NOLOCK)
                LEFT JOIN
                    GS300GP.dbo.tblCampanhaTelevendasResultado tblCamTelRes (NOLOCK) ON tblCamTelRes.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
                LEFT JOIN
                    GS300GP.dbo.tblCampanhaTelevendasMeta tblCamTelMet (NOLOCK) ON tblCamTelMet.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
                WHERE
                    CAST(tblCamTel.DataCompetencia AS DATE) >= @dataMinima
                GROUP BY
                    tblCamTelRes.IDCampanhaTelevendas,
                    tblCamTelRes.DescricaoCampanha,
                    tblCamTel.IDCampanhaTelevendasTipoApuracao
                ) tblPre ON tblPre.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
            LEFT JOIN
                GS300GP.dbo.tblCampanhaTelevendasFaixaPremiacao tblCamTelFaiPre (NOLOCK) ON tblCamTelFaiPre.IDCampanhaTelevendas = tblCamTel.IDCampanhaTelevendas
            WHERE
                CAST(tblCamTel.DataCompetencia AS DATE) >= @dataMinima AND
                tblCamTelRes.TipoPessoa = 1 AND /* SUPERVISOR */
                tblCamTel.IDCampanhaTelevendasPeriodoCompetenciaSituacao <> 4 /* CANCELADO */
            GROUP BY
                tblCamTel.IDCampanhaTelevendas,
                tblCamTelRes.DescricaoCampanha,
                tblCamTelRes.DataCompetencia,
                tblCamTel.IDCampanhaTelevendasTipoApuracao,
                tblCamTelMet.MetaValor,
                tblCamTelRes.ValorApurado,
                tblPre.PremiacaoTotal,
                tblCamTel.Observacao,
                tblCamTelRes.ValorApuradoBees
            """;

        private readonly AppDbContext _context;

        public CampaignSummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CampaignSummary>> GetSummaryAsync(DateTime competenceDateFrom)
        {
            var parameters = new object[]
            {
                new SqlParameter("@dataMinima", competenceDateFrom.Date)
            };

            return await _context.Database
                .SqlQueryRaw<CampaignSummary>(Sql, parameters)
                .ToListAsync();
        }
    }
}
