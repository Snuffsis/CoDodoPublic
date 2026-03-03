using Domain.Opportunities;
using SharedKernel;

namespace Domain.Processes;

public sealed class Process : Entity
{
    public string Name { get; set; } = string.Empty;
    public string OpportunityUri { get; set; } = string.Empty;
    public Opportunity Opportunity { get; set; } = new();
    public Status Status { get; set; }
}