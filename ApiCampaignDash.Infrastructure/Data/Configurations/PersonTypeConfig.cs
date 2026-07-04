using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class PersonTypeConfig : IEntityTypeConfiguration<PersonType>
    {
        public void Configure(EntityTypeBuilder<PersonType> builder)
        {
            builder.ToTable("tblCampanhaTelevendasTipoPessoa");
            builder.HasKey(x => x.IdPersonType);
            builder.Property(x => x.IdPersonType).HasColumnName("IDCampanhaTelevendasTipoPessoa");
            builder.Property(x => x.Description).HasColumnName("Descricao").HasMaxLength(200);
        }
    }
}
