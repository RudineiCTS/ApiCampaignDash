using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("tblCampanhaTelevendasProduto");
            builder.HasKey(x => new { x.IdCampaign, x.IdProduct });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdProduct).HasColumnName("IDProduto");
            builder.Property(x => x.IsValid).HasColumnName("Contem");
            // Name vem de join com a tabela mestre de produtos, nao existe na tabela de campanha
            builder.Ignore(x => x.Name);
        }
    }
}
