namespace CoDodoWeb.Models;

public class CreateOpportunityRequest
{
  public string UriForAssignment { get; set; } = string.Empty;
  public string Company { get; set; } = string.Empty;
  public string Capability { get; set; } = string.Empty;
  public string NameOfSalesLead { get; set; } = string.Empty;
  public decimal HourlyRateInSek { get; set; }
}
