using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Opportunities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Opportunities.Update;

internal sealed class UpdateOpportunityCommandHandler(
    IApplicationDbContext context)
    : ICommandHandler<UpdateOpportunityCommand>
{
    public async Task<Result> Handle(UpdateOpportunityCommand command, CancellationToken cancellationToken)
    {
        Opportunity? opportunity = await context.Opportunities
            .SingleOrDefaultAsync(t => t.Id == command.OpportunityId, cancellationToken);

        if (opportunity is null)
        {
            return Result.Failure(OpportunityErrors.NotFound(command.OpportunityId));
        }

        if (!string.IsNullOrWhiteSpace(command.Company))
        {
          opportunity.Company = command.Company;
        }

        if (!string.IsNullOrWhiteSpace(command.Capability))
        {
          opportunity.Capability = command.Capability;
        }

        if (!string.IsNullOrWhiteSpace(command.NameOfSalesLead))
        {
          opportunity.NameOfSalesLead = command.NameOfSalesLead;
        }

        if (command.HourlyRateInSek.HasValue)
        {
          opportunity.HourlyRateInSek = command.HourlyRateInSek.Value;
        }

        await context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
