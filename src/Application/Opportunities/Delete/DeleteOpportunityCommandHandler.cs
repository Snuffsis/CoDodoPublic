using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Opportunities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Opportunities.Delete;

internal sealed class DeleteOpportunityCommandHandler(
  IApplicationDbContext context) : ICommandHandler<DeleteOpportunityCommand>
{
  public async Task<Result> Handle(DeleteOpportunityCommand command, CancellationToken cancellationToken)
  {
    Opportunity? opportunity = await context.Opportunities
      .SingleOrDefaultAsync(o => o.Id == command.OpportunityId, cancellationToken);

    if (opportunity == null)
    {
      return Result.Failure(OpportunityErrors.NotFound(command.OpportunityId));
    }

    context.Opportunities.Remove(opportunity);

    await context.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}