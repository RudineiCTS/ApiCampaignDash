using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    // A tabela tblCampanhaTelevendasBaseDigitadoras nao expoe uma chave explicita no diagrama;
    // assumimos a chave natural (supervisor + pessoa de vendas). Ajustar se necessario.
    public class SalesOperatorBaseConfig : IEntityTypeConfiguration<SalesOperatorBase>
    {
        public void Configure(EntityTypeBuilder<SalesOperatorBase> builder)
        {
            builder.ToTable("tblCampanhaTelevendasBaseDigitadoras");
            builder.HasKey(x => new { x.IdSupervisor, x.IdPersonSales });
            builder.Property(x => x.IdSupervisor).HasColumnName("IDSupervisor");
            builder.Property(x => x.SupervisorName).HasColumnName("NomeSupervisor").HasMaxLength(200);
            builder.Property(x => x.IdPersonSales).HasColumnName("IDPessoaTelevendas");
            builder.Property(x => x.OperatorName).HasColumnName("NomeDigitador").HasMaxLength(200);
            builder.Property(x => x.OperatorUser).HasColumnName("UsuarioDigitador").HasMaxLength(100);
            builder.Property(x => x.IdSector).HasColumnName("IDSetor");
        }
    }
}
