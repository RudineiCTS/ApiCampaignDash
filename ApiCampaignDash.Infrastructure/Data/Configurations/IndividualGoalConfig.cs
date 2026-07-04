using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class IndividualGoalConfig : IEntityTypeConfiguration<IndividualGoal>
    {
        public void Configure(EntityTypeBuilder<IndividualGoal> builder)
        {
            builder.ToTable("tblCampanhaTelevendasMetaInd");
            builder.HasKey(x => new { x.IdCampaign, x.IdPerson });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdPerson).HasColumnName("IDPessoa");
            builder.Property(x => x.IdSupervisor).HasColumnName("IDSupervisor");
            builder.Property(x => x.PersonType).HasColumnName("TipoPessoa");
            builder.Property(x => x.GoalValue).HasColumnName("MetaValor");
            builder.Property(x => x.Trigger).HasColumnName("Gatilho");
            builder.Property(x => x.IdManager).HasColumnName("IDGerente");
        }
    }
}
