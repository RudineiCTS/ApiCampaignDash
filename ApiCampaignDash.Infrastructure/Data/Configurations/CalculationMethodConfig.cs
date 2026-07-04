using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class CalculationMethodConfig : IEntityTypeConfiguration<CalculationMethod>
    {
        public void Configure(EntityTypeBuilder<CalculationMethod> builder)
        {
            builder.ToTable("tblCampanhaTelevendasTipoCalculo");
            builder.HasKey(x => x.IdCalculationMethod);
            builder.Property(x => x.IdCalculationMethod).HasColumnName("IDCampanhaTelevendasTipoCalculo");
            builder.Property(x => x.Description).HasColumnName("DescricaoTipoCalculo").HasMaxLength(200);
        }
    }
}
