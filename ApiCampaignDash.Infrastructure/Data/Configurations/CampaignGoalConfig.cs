using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class CampaignGoalConfig : IEntityTypeConfiguration<CampaignGoal>
    {
        public void Configure(EntityTypeBuilder<CampaignGoal> builder)
        {
            builder.ToTable("tblCampanhaTelevendasMeta");
            builder.HasKey(x => x.IdCampaign);
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.GoalValue).HasColumnName("MetaValor");
            builder.Property(x => x.PositivationTrigger).HasColumnName("GatilhoPositivacao");
            builder.Property(x => x.SalesTrigger).HasColumnName("GatilhoVenda");
        }
    }
}
