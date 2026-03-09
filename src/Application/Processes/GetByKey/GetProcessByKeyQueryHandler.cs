using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Opportunities.Get;
using Application.Processes.GetById;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Processes.GetByKey;

internal sealed class GetProcessByKeyQueryHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : IQueryHandler<GetProcessByKeyQuery, ProcessResponse>
{
  public async Task<Result<ProcessResponse>> Handle(GetProcessByKeyQuery query,
    CancellationToken cancellationToken)
  {
    var process = await context.Processes
      .Include(p => p.Opportunity)
      .FirstOrDefaultAsync(p => 
        p.Name == query.Name &&
        p.OpportunityUri == query.UriForAssignment, cancellationToken);
    if (process == null)
    {
      return Result.Failure<ProcessResponse>(ProcessErrors.NotFoundByKey(query.Name, query.UriForAssignment));
    }

    var processResponse = new ProcessResponse
    {
      Id = process.Id,
      Name = process.Name,
      UriForAssignment = process.OpportunityUri,
      Status =  process.Status,
      Opportunity = new OpportunityResponse
      {
        Id = process.Opportunity.Id,
        UriForAssignment = process.Opportunity.UriForAssignment,
        Company = process.Opportunity.Company,
        Capability = process.Opportunity.Capability,
        NameOfSalesLead = process.Opportunity.NameOfSalesLead,
        HourlyRateInSek = process.Opportunity.HourlyRateInSek,
        CreatedAt = process.Opportunity.CreatedAt,
        UpdatedAt = process.Opportunity.UpdatedAt,
        DaysSinceUpdate = DaysSince(process.Opportunity.UpdatedAt),
        DaysSinceCreation = DaysSince(process.Opportunity.CreatedAt),
      },
      CreatedAt = process.CreatedAt,
      UpdatedAt = process.UpdatedAt,
      DaysSinceUpdate = DaysSince(process.Opportunity.UpdatedAt),
      DaysSinceCreation = DaysSince(process.Opportunity.CreatedAt)
    };

    return processResponse;
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