namespace CoDodoWeb.Models;

public class CoDodoIndexViewModel
{
  public IReadOnlyList<OpportunityModel> Opportunities { get; set; } = [];
  public IReadOnlyDictionary<Guid, int> ProcessCountsByOpportunity { get; set; } = new Dictionary<Guid, int>();
  public string? ErrorMessage { get; set; }
}
