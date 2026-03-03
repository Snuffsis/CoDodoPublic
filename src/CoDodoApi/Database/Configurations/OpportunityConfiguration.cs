using Domain.Opportunities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoDodoApi.Database.Configurations;

public class OpportunityConfiguration :IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.Id)
            .IsUnique();
        builder.HasIndex(o => o.UriForAssignment)
            .IsUnique();
    }
}