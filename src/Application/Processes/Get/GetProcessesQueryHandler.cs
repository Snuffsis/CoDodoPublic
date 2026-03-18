using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Opportunities.GetById;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using OpportunityResponse = Application.Opportunities.Get.OpportunityResponse;

namespace Application.Processes.Get;

internal sealed class GetProcessesQueryHandler(
  IApplicationDbContext context,
  IDateTimeProvider dateTimeProvider)
  : IQueryHandler<GetProcessesQuery, List<ProcessResponse>>
{
  public async Task<Result<List<ProcessResponse>>> Handle(GetProcessesQuery query,
    CancellationToken cancellationToken)
  {
    List<ProcessResponse> processes = await context.Processes
      .Select(p => new ProcessResponse
      {
        Id = p.Id,
        Name =  p.Name,
        UriForAssignment = p.OpportunityUri,
        Status = p.Status,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        Opportunity = new OpportunityResponse
        {
          Id = p.Opportunity.Id,
          UriForAssignment = p.Opportunity.UriForAssignment,
          Company = p.Opportunity.Company,
          Capability = p.Opportunity.Capability,
          NameOfSalesLead = p.Opportunity.NameOfSalesLead,
          HourlyRateInSek = p.Opportunity.HourlyRateInSek,
          CreatedAt = p.Opportunity.CreatedAt,
          UpdatedAt = p.Opportunity.UpdatedAt,
        },
      })
      .ToListAsync(cancellationToken);
    
    processes = processes
      .Select(p => new ProcessResponse
      {
        Id = p.Id,
        Name = p.Name,
        UriForAssignment = p.UriForAssignment,
        Status = p.Status,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt,
        DaysSinceCreation = DaysSince(p.CreatedAt),
        DaysSinceUpdate = DaysSince(p.UpdatedAt),

        Opportunity = new OpportunityResponse
        {
          Id = p.Opportunity.Id,
          UriForAssignment = p.Opportunity.UriForAssignment,
          Company = p.Opportunity.Company,
          Capability = p.Opportunity.Capability,
          NameOfSalesLead = p.Opportunity.NameOfSalesLead,
          HourlyRateInSek = p.Opportunity.HourlyRateInSek,
          CreatedAt = p.Opportunity.CreatedAt,
          UpdatedAt = p.Opportunity.UpdatedAt,
          DaysSinceCreation = DaysSince(p.Opportunity.CreatedAt),
          DaysSinceUpdate = DaysSince(p.Opportunity.UpdatedAt),
        }
      })
      .ToList();

    return processes;
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