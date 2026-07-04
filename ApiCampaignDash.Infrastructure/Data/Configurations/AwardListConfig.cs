using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    // A tabela tblCampanhaTelevendasFaixaPremiacao nao expoe uma chave explicita no diagrama;
    // assumimos a chave natural (campanha + tipo de pessoa + inicio do ranking). Ajustar se necessario.
    public class AwardListConfig : IEntityTypeConfiguration<AwardList>
    {
        public void Configure(EntityTypeBuilder<AwardList> builder)
        {
            builder.ToTable("tblCampanhaTelevendasFaixaPremiacao");
            builder.HasKey(x => new { x.IdCampaign, x.PersonType, x.RankingStart });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.StartValue).HasColumnName("ValorInicio");
            builder.Property(x => x.EndValue).HasColumnName("ValorFim");
            builder.Property(x => x.RankingStart).HasColumnName("RankingInicio");
            builder.Property(x => x.RankingEnd).HasColumnName("RankingFim");
            builder.Property(x => x.AwardValue).HasColumnName("PremioValor");
            builder.Property(x => x.AwardLimit).HasColumnName("LimitePremio");
            builder.Property(x => x.PersonType).HasColumnName("TipoPessoa");
        }
    }
}
