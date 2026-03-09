using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Opportunities.Get;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Processes.GetById;

internal sealed class GetProcessByIdQueryHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : IQueryHandler<GetProcessByIdQuery, ProcessResponse>
{
  public async Task<Result<ProcessResponse>> Handle(GetProcessByIdQuery query,
    CancellationToken cancellationToken)
  {
    var process = await context.Processes
      .Include(p => p.Opportunity)
      .FirstOrDefaultAsync(p => p.Id == query.ProcessId, cancellationToken);
    if (process == null)
    {
      return Result.Failure<ProcessResponse>(ProcessErrors.NotFound(query.ProcessId));
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