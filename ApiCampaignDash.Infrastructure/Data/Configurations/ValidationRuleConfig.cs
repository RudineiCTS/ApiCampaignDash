using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class ValidationRuleConfig : IEntityTypeConfiguration<ValidationRule>
    {
        public void Configure(EntityTypeBuilder<ValidationRule> builder)
        {
            builder.ToTable("tblCampanhaTelevendasRegraValidacao");
            builder.HasKey(x => new { x.IdCampaign, x.IdCampaignToValidate });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdCampaignToValidate).HasColumnName("IDCampanhaTelevendasValidar");
            builder.Property(x => x.ValidationResult).HasColumnName("ResultadoValidacao");
        }
    }
}
