using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Processes.Update;

internal sealed class UpdateProcessCommandHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : ICommandHandler<UpdateProcessCommand>
{
  public async Task<Result> Handle(UpdateProcessCommand command, CancellationToken cancellationToken)
  {
    Process? process = await context.Processes
      .SingleOrDefaultAsync(t =>
          t.Name == command.Name &&
          t.OpportunityUri == command.OpportunityId,
        cancellationToken);

    if (process is null)
    {
      return Result.Failure(ProcessErrors.NotFoundByKey(command.Name, command.OpportunityId));
    }

    if (command.Status.HasValue)
    {
      process.Status = command.Status.Value;
      process.UpdatedAt = dateTimeProvider.UtcNow;
    }

    await context.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}