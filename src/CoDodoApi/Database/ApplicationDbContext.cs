using CoDodoApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Database;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
        
    }
    
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<Process> Processes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}