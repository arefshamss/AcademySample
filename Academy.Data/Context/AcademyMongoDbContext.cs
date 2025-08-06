using Academy.Domain.Models.AdminSearch;
using Academy.Domain.Models.Logs;
using Academy.Domain.Models.UserActivities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Academy.Data.Context;

public class AcademyMongoDbContext(DbContextOptions<AcademyMongoDbContext> options) : DbContext(options)
{
    public DbSet<UserActivity> UserActivities { get; set; }

    public DbSet<ApplicationLog> ApplicationLogs { get; set; }

    public DbSet<AdminSearchEntry> AdminSearchEntries { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        modelBuilder.Entity<UserActivity>().ToCollection("UserActivities");
        modelBuilder.Entity<ApplicationLog>().ToCollection("ApplicationLogs");
        modelBuilder.Entity<AdminSearchEntry>().ToCollection("AdminSearchEntries");
        base.OnModelCreating(modelBuilder);
    }
    
}