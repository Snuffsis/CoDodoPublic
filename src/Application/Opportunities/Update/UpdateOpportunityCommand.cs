using Application.Abstractions.Messaging;

namespace Application.Opportunities.Update;

public sealed record UpdateOpportunityCommand(
    Guid OpportunityId,
    string? Company,
    string? Capability,
    string? NameOfSalesLead,
    decimal? HourlyRateInSek) : ICommand;