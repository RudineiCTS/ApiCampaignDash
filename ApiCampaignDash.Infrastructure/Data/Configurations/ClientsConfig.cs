using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ClientsConfig : IEntityTypeConfiguration<Clients>
    {
        public void Configure(EntityTypeBuilder<Clients> builder)
        {
            builder.ToTable("tblCampanhaTelevendasCliente");
            builder.HasKey(x => new { x.IdCampaign, x.IdClients });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdClients).HasColumnName("IDCliente");
            builder.Property(x => x.IsValid).HasColumnName("Contem");
            // ClientName vem de join com a tabela mestre de clientes, nao existe na tabela de campanha
            builder.Ignore(x => x.ClientName);
        }
    }
}
