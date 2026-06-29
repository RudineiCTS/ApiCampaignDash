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
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IDCampanhaComercial");
            builder.Property(x => x.Nome).HasColumnName("NomeCampanha").HasMaxLength(100);
        }
    }
}
