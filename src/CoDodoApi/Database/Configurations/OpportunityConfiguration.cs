using CoDodoApi.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoDodoApi.Database.Configurations;

public class OpportunityConfiguration :IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.HasKey(o => o.UriForAssignment);
        builder.HasIndex(o => o.UriForAssignment)
            .IsUnique();
    }
}