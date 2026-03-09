using Domain.Opportunities;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Process> Processes { get; }
    DbSet<Opportunity> Opportunities { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
