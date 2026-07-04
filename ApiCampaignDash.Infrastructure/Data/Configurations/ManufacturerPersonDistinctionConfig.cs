using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ManufacturerPersonDistinctionConfig : IEntityTypeConfiguration<ManufacturerPersonDistinction>
    {
        public void Configure(EntityTypeBuilder<ManufacturerPersonDistinction> builder)
        {
            builder.ToTable("tblCampanhaTelevendasDistincaoPessoaFabricante");
            builder.HasKey(x => new { x.IdManufacturer, x.IdPerson });
            builder.Property(x => x.IdManufacturer).HasColumnName("IDFabricante");
            builder.Property(x => x.ManufacturerDescription).HasColumnName("DescricaoFabricante").HasMaxLength(200);
            builder.Property(x => x.IdPerson).HasColumnName("IDPessoa");
            builder.Property(x => x.IdOperator).HasColumnName("IDDigitador");
        }
    }
}
