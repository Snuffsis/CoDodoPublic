using CoDodoApi.Database.Entities;
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
        builder.HasOne(p => p.Opportunity)
            .WithMany()
            .HasForeignKey(p => p.OpportunityUri)
            .HasPrincipalKey(o => o.UriForAssignment);
    }
}