using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Opportunities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Opportunities.Create;

internal sealed class CreateOpportunityCommandHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : ICommandHandler<CreateOpportunityCommand, Guid>
{
  public async Task<Result<Guid>> Handle(CreateOpportunityCommand command, CancellationToken cancellationToken)
  {
    if (!await context.Opportunities.AnyAsync(o => o.UriForAssignment == command.UriForAssignment, cancellationToken))
    {
      return Result.Failure<Guid>(OpportunityErrors.UriNotUnique(command.UriForAssignment));
    }

    var opportunity = new Opportunity
    {
      Id = Guid.NewGuid(),
      UriForAssignment = command.UriForAssignment,
      Company = command.Company,
      Capability = command.Capability,
      NameOfSalesLead = command.NameOfSalesLead,
      HourlyRateInSek = command.HourlyRateInSek,
      CreatedAt = dateTimeProvider.UtcNow,
      UpdatedAt = dateTimeProvider.UtcNow,
    };

    context.Opportunities.Add(opportunity);

    await context.SaveChangesAsync(cancellationToken);

    return opportunity.Id;
  }
}