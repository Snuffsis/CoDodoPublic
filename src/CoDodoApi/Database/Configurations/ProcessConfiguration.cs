using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoDodoApi.Database.Configurations;

public class ProcessConfiguration :IEntityTypeConfiguration<Process>
{
    public void Configure(EntityTypeBuilder<Process> builder)
    {
        builder.HasKey(p => new
        {
            p.Name,
            p.OpportunityUri
        });
    }
}