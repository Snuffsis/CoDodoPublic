using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Opportunities;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Processes.Create;

internal sealed class CreateProcessCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateProcessCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProcessCommand command, CancellationToken cancellationToken)
    {
      if (!await context.Processes.AnyAsync(p => p.Name == command.Name && p.OpportunityUri == command.UriForAssignment, cancellationToken))
      {
        return Result.Failure<Guid>(ProcessErrors.KeyNotUnique());
      }

      var process = new Process
      {
        Id = Guid.NewGuid(),
        CreatedAt = dateTimeProvider.UtcNow,
        UpdatedAt = dateTimeProvider.UtcNow,
        Name = command.Name,
        OpportunityUri = command.UriForAssignment,
        Status = command.Status
      };

      context.Processes.Add(process);

      await context.SaveChangesAsync(cancellationToken);

      return process.Id;
      
    }
}
