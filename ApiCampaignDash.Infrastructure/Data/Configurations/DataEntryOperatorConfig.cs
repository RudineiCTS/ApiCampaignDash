using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiCampaignDash.Infrastructure.Data.Configurations
{
    public class DataEntryOperatorConfig : IEntityTypeConfiguration<DataEntryOperator>
    {
        public void Configure(EntityTypeBuilder<DataEntryOperator> builder)
        {
            builder.ToTable("tblCampanhaTelevendasDigitadora");
            builder.HasKey(x => new { x.IdCampaign, x.IdPersonSales });
            builder.Property(x => x.IdCampaign).HasColumnName("IDCampanhaTelevendas");
            builder.Property(x => x.IdPersonSales).HasColumnName("IDPessoaTelevendas");
            builder.Property(x => x.IsValid).HasColumnName("Contem");
        }
    }
}
