using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class CompetencePeriodStatusConfig : IEntityTypeConfiguration<CompetencePeriodStatus>
    {
        public void Configure(EntityTypeBuilder<CompetencePeriodStatus> builder)
        {
            builder.ToTable("tblCampanhaTelevendasPeriodoCompetenciaSituacao");
            builder.HasKey(x => x.IdCompetencePeriodStatus);
            builder.Property(x => x.IdCompetencePeriodStatus).HasColumnName("IDCampanhaTelevendasPeriodoCompetenciaSituacao");
            builder.Property(x => x.Description).HasColumnName("DescricaoPeriodoCompetenciaSituacao").HasMaxLength(200);
        }
    }
}
