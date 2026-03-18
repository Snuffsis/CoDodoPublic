namespace CoDodoWeb.Models;

public class OpportunityDetailsViewModel
{
  public OpportunityModel? Opportunity { get; set; }
  public IReadOnlyList<ProcessModel> Processes { get; set; } = [];
  public string? ErrorMessage { get; set; }
  public string? SuccessMessage { get; set; }
}
