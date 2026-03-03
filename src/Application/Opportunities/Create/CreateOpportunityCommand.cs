using Application.Abstractions.Messaging;

namespace Application.Opportunities.Create;

public sealed record CreateOpportunityCommand(
  string UriForAssignment, 
  string Company,
  string Capability,
  string NameOfSalesLead,
  decimal HourlyRateInSek) : ICommand<Guid>;