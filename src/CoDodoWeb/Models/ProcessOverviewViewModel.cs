namespace CoDodoWeb.Models;

public class ProcessOverviewViewModel
{
  public IReadOnlyList<ProcessModel> Processes { get; set; } = [];
  public ProcessStatus? StatusFilter { get; set; }
  public string PersonNameFilter { get; set; } = string.Empty;
  public string? ErrorMessage { get; set; }
}
