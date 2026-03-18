namespace CoDodoWeb.Models;

public class OpportunityModel
{
  public Guid Id { get; set; }
  public string UriForAssignment { get; set; } = string.Empty;
  public string Company { get; set; } = string.Empty;
  public string Capability { get; set; } = string.Empty;
  public string NameOfSalesLead { get; set; } = string.Empty;
  public decimal HourlyRateInSek { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public int? DaysSinceUpdate { get; set; }
  public int? DaysSinceCreation { get; set; }
}
