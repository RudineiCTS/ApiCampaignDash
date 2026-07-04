using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class CampaignResultConfig : IEntityTypeConfiguration<CampaignResult>
    {
        public void Configure(EntityTypeBuilder<CampaignResult> builder)
        {
            builder.ToTable("tblCampanhaTelevendasResultado");
            builder.HasKey(x => new { x.IdCampaign, x.IdPersonSales });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.CompetenceDate).HasColumnName("DataCompetencia");
            builder.Property(x => x.CampaignDescription).HasColumnName("DescricaoCampanha").HasMaxLength(200);
            builder.Property(x => x.IdPersonSales).HasColumnName("IDPessoaTelevendas");
            builder.Property(x => x.OperatorName).HasColumnName("NomeDigitador").HasMaxLength(200);
            builder.Property(x => x.Ranking).HasColumnName("Ranking");
            builder.Property(x => x.AssessedValue).HasColumnName("ValorApurado");
            builder.Property(x => x.Award).HasColumnName("Premiacao");
            builder.Property(x => x.CalculationLog).HasColumnName("LogCalculo");
            builder.Property(x => x.CalculationDate).HasColumnName("DataCalculo");
            builder.Property(x => x.PersonType).HasColumnName("TipoPessoa");
            builder.Property(x => x.IdSupervisor).HasColumnName("IDSupervisor");
            builder.Property(x => x.SupervisorName).HasColumnName("NomeSupervisor").HasMaxLength(200);
            builder.Property(x => x.TotalAchieved).HasColumnName("RealizadoTotal");
            builder.Property(x => x.IndividualTarget).HasColumnName("ObjetivoIndividual");
            builder.Property(x => x.PercentageAchieved).HasColumnName("PercentualRealizadoIndividual");
            builder.Property(x => x.AssessedValueBees).HasColumnName("ValorApuradoBees");
        }
    }
}
