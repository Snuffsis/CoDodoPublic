namespace CoDodoWeb.Models;

public class CreateProcessRequest
{
  public string Name { get; set; } = string.Empty;
  public string UriForAssignment { get; set; } = string.Empty;
  public ProcessStatus Status { get; set; }
}
