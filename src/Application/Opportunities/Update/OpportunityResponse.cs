namespace Application.Opportunities.Update;

public sealed class OpportunityResponse
{
    public Guid Id { get; set; }
    public string UriForAssignment { get; set; }
    public string Company { get; set; }
    public string Capability { get; set; }
    public string NameOfSalesLead { get; set; }
    public decimal HourlyRateInSek { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? DaysSinceUpdate { get; set; }
    public int? DaysSinceCreation { get; set; }
}

