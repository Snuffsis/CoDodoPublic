using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Processes.Delete;

internal sealed class DeleteProcessCommandHandler(
  IApplicationDbContext context)
    : ICommandHandler<DeleteProcessCommand>
{
    public async Task<Result> Handle(DeleteProcessCommand command, CancellationToken cancellationToken)
    {
        Process? process = await context.Processes
            .SingleOrDefaultAsync(t => t.Id == command.ProcessId, cancellationToken);

        if (process is null)
        {
            return Result.Failure(ProcessErrors.NotFound(command.ProcessId));
        }

        context.Processes.Remove(process);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
