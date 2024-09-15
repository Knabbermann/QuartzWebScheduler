using QuartzWebScheduler.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuartzWebScheduler.DataAccess.DbContext
{
    public class WebDbContext : IdentityDbContext<WebUser>
    {
        public WebDbContext(DbContextOptions<WebDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Custom Entities
            modelBuilder.Entity<Log>()
                .ToTable("Logs");
            modelBuilder.Entity<QuartzLog>()
                .ToTable("QuartzLogs");
            modelBuilder.Entity<QuartzJobConfig>()
                .ToTable("QuartzJobConfigs");
            modelBuilder.Entity<QuartzGroup>()
                .ToTable("QuartzGroups");

            base.OnModelCreating(modelBuilder);
        }
    }
}
