using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ManufacturerConfig : IEntityTypeConfiguration<Manufacturer>
    {
        public void Configure(EntityTypeBuilder<Manufacturer> builder)
        {
            builder.ToTable("tblCampanhaTelevendasFabricante");
            builder.HasKey(x => new { x.IdCampaign, x.IdManufacturer });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdManufacturer).HasColumnName("IDFabricante");
            builder.Property(x => x.IsValid).HasColumnName("Contem");
            // Name vem de join com a tabela mestre de fabricantes, nao existe na tabela de campanha
            builder.Ignore(x => x.Name);
        }
    }
}
