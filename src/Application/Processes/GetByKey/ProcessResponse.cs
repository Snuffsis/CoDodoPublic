using Application.Opportunities.Get;
using Domain.Processes;

namespace Application.Processes.GetByKey;

public sealed class ProcessResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; }
  public string UriForAssignment { get; set; }
  public Status Status { get; set; }
  public OpportunityResponse Opportunity { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public int? DaysSinceUpdate { get; set; }
  public int? DaysSinceCreation { get; set; }
}

