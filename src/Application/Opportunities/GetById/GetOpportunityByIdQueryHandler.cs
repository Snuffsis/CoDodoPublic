using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Opportunities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Opportunities.GetById;

internal sealed class GetOpportunityByIdQueryHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : IQueryHandler<GetOpportunityByIdQuery, OpportunityResponse>
{
  public async Task<Result<OpportunityResponse>> Handle(
    GetOpportunityByIdQuery query,
    CancellationToken cancellationToken)
  {
    var opportunity = await context.Opportunities.FirstOrDefaultAsync(o => o.Id == query.OpportunityId, cancellationToken);
    if (opportunity == null)
    {
      return Result.Failure<OpportunityResponse>(OpportunityErrors.NotFound(query.OpportunityId));
    }

    var response = new OpportunityResponse
    {
      Id = opportunity.Id,
      UriForAssignment = opportunity.UriForAssignment,
      Company = opportunity.Company,
      Capability = opportunity.Capability,
      NameOfSalesLead = opportunity.NameOfSalesLead,
      HourlyRateInSek = opportunity.HourlyRateInSek,
      CreatedAt = opportunity.CreatedAt,
      UpdatedAt = opportunity.UpdatedAt,
      DaysSinceUpdate = DaysSince(opportunity.UpdatedAt),
      DaysSinceCreation = DaysSince(opportunity.CreatedAt),
    };

    return response;
  }
  
  private int DaysSince(DateTime date)
  {
    TimeSpan diff = dateTimeProvider.UtcNow - date;

    return NumberOfWholeDays(diff);
  }

  private static int NumberOfWholeDays(TimeSpan diff)
  {
    double numberOfDays = diff.TotalDays;

    return (int)numberOfDays;
  }
}