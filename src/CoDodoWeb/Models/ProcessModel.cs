namespace CoDodoWeb.Models;

public class ProcessModel
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string UriForAssignment { get; set; } = string.Empty;
  public ProcessStatus Status { get; set; }
  public OpportunityModel? Opportunity { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public int? DaysSinceUpdate { get; set; }
  public int? DaysSinceCreation { get; set; }
}
