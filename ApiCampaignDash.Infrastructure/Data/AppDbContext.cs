using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<CampaignResume> Campanhas { get; set; }        
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<PersonType> PersonTypes { get; set; }
        public DbSet<CompetencePeriodStatus> CompetencePeriodStatuses { get; set; }
        public DbSet<AssessmentType> AssessmentTypes { get; set; }
        public DbSet<CalculationMethod> CalculationMethods { get; set; }
        public DbSet<IndividualGoal> IndividualGoals { get; set; }
        public DbSet<CampaignGoal> CampaignGoals { get; set; }
        public DbSet<AwardList> AwardLists { get; set; }
        public DbSet<CampaignResult> CampaignResults { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<DataEntryOperator> DataEntryOperators { get; set; }
        public DbSet<ValidationRule> ValidationRules { get; set; }
        public DbSet<SalesOperatorBase> SalesOperatorBases { get; set; }
        public DbSet<ManufacturerPersonDistinction> ManufacturerPersonDistinctions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ProductLine> ProductLines { get; set; }
        public DbSet<Clients> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


    }
}
