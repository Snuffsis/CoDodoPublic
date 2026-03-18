namespace CoDodoWeb.Models;

public class UpdateProcessStatusRequest
{
  public Guid OpportunityId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string UriForAssignment { get; set; } = string.Empty;
  public ProcessStatus Status { get; set; }
}
