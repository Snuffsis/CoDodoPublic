using Application.Abstractions.Messaging;

namespace Application.Opportunities.Delete;

public sealed record DeleteOpportunityCommand(Guid OpportunityId) : ICommand;
