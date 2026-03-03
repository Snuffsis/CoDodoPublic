using SharedKernel;

namespace Domain.Opportunities;

public sealed class Opportunity : Entity
{
  public string UriForAssignment { get; set; } = string.Empty;
  public string Company { get; set; } = string.Empty;
  public string Capability { get; set; } = string.Empty;
  public string NameOfSalesLead { get; set; } = string.Empty;
  public decimal HourlyRateInSek { get; set; }
}