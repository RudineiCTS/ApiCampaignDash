using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class AssessmentTypeConfig : IEntityTypeConfiguration<AssessmentType>
    {
        public void Configure(EntityTypeBuilder<AssessmentType> builder)
        {
            builder.ToTable("tblCampanhaTelevendasTipoApuracao");
            builder.HasKey(x => x.IdAssessmentType);
            builder.Property(x => x.IdAssessmentType).HasColumnName("IDCampanhaTelevendasTipoApuracao");
            builder.Property(x => x.Description).HasColumnName("DescricaoTipoApuracao").HasMaxLength(200);
        }
    }
}
