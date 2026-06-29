using ApiCampaignDash.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiCampaignDash.Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }       
        public DbSet<CampaignResume> Campanhas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


    }
}
