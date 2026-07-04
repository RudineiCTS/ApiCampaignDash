using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class CampaignResumeConfig: IEntityTypeConfiguration<CampaignResume>
    {
        public void Configure(EntityTypeBuilder<CampaignResume> builder)
        {
            builder.ToTable("tblCampanhaComercial");
            builder.HasKey(x => x.IdCampaign);
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaComercial");
            builder.Property(x => x.TitleCampaign).HasColumnName("NomeCampanha").HasMaxLength(100);
        }
    }
}
