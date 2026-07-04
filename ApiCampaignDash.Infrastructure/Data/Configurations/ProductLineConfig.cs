using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ProductLineConfig : IEntityTypeConfiguration<ProductLine>
    {
        public void Configure(EntityTypeBuilder<ProductLine> builder)
        {
            builder.ToTable("tblCampanhaTelevendasLinhaProduto");
            builder.HasKey(x => new { x.IdCampaign, x.IdProductLine });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdProductLine).HasColumnName("IDLinhaProduto");
            builder.Property(x => x.IsValid).HasColumnName("Contem");
            // Name vem de join com a tabela mestre de linhas de produto, nao existe na tabela de campanha
            builder.Ignore(x => x.Name);
        }
    }
}
