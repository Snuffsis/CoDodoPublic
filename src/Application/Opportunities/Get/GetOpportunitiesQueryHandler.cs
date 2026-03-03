using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Opportunities.Get;

internal sealed class GetOpportunitiesQueryHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : IQueryHandler<GetOpportunitiesQuery, List<OpportunityResponse>>
{
  public async Task<Result<List<OpportunityResponse>>> Handle(
    GetOpportunitiesQuery query,
    CancellationToken cancellationToken)
  {
    List<OpportunityResponse> raw = await context.Opportunities
      .Select(o => new OpportunityResponse
      {
        Id = o.Id,
        UriForAssignment = o.UriForAssignment,
        Company = o.Company,
        Capability = o.Capability,
        NameOfSalesLead = o.NameOfSalesLead,
        HourlyRateInSek = o.HourlyRateInSek,
        CreatedAt = o.CreatedAt,
        UpdatedAt = o.UpdatedAt
      })
      .ToListAsync(cancellationToken);

    var opportunities = raw
      .Select(o => new OpportunityResponse
      {
        Id = o.Id,
        UriForAssignment = o.UriForAssignment,
        Company = o.Company,
        Capability = o.Capability,
        NameOfSalesLead = o.NameOfSalesLead,
        HourlyRateInSek = o.HourlyRateInSek,
        CreatedAt = o.CreatedAt,
        UpdatedAt = o.UpdatedAt,
        DaysSinceCreation = DaysSince(o.CreatedAt),
        DaysSinceUpdate =  DaysSince(o.UpdatedAt)
      })
      .ToList();

    return opportunities;
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