using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class CampaignConfig : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("tblCampanhaTelevendas");
            builder.HasKey(x => x.IdCampaign);
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.CompetenceDate).HasColumnName("DataCompetencia");
            builder.Property(x => x.IdCompetencePeriodStatus).HasColumnName("IDCampanhaTelevendasPeriodoCompetenciaSituacao");
            builder.Property(x => x.StartDate).HasColumnName("DataInicioApuracao");
            builder.Property(x => x.EndDate).HasColumnName("DataFimApuracao");
            builder.Property(x => x.TotalRanking).HasColumnName("TotalRanking");
            builder.Property(x => x.Description).HasColumnName("DescricaoCampanha").HasMaxLength(150);
            builder.Property(x => x.IdAssessmentType).HasColumnName("IDCampanhaTelevendasTipoApuracao");
            builder.Property(x => x.IdCalculationMethod).HasColumnName("IDCampanhaTelevendasTipoCalculo");
            builder.Property(x => x.ValidationRule).HasColumnName("RegraValidacao");
            builder.Property(x => x.ValueType).HasColumnName("TipoValor");
            builder.Property(x => x.EarlyEndDate).HasColumnName("DataFimAntecipada");
            builder.Property(x => x.Notes).HasColumnName("Observacao").HasMaxLength(150);
            // ConsideraExclusivas e TipoCampanhaTelevendas sao armazenadas como int/bit no banco;
            // convertidas para bool? no dominio por representarem flags.
            builder.Property(x => x.ConsidersExclusives).HasColumnName("ConsideraExclusivas").HasConversion<int?>();
            builder.Property(x => x.CampaignType).HasColumnName("TipoCampanhaTelevendas");
        }
    }
}
