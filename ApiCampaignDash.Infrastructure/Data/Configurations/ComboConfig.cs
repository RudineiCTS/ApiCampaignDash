using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ComboConfig : IEntityTypeConfiguration<Combo>
    {
        public void Configure(EntityTypeBuilder<Combo> builder)
        {
            builder.ToTable("tblCampanhaTelevendasCombo");
            builder.HasKey(x => new { x.IdCampaign, x.IdCombo });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdCombo).HasColumnName("IDCombo");
            builder.Property(x => x.IsValid).HasColumnName("Contem");
        }
    }
}
