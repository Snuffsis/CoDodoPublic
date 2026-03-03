using Application.Abstractions.Data;
using Domain.Opportunities;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Database;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
        
    }
    
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<Process> Processes { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
      configurationBuilder
        .Properties<Enum>()
        .HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}