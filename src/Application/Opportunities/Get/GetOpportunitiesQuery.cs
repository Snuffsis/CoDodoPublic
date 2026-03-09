using Application.Abstractions.Messaging;

namespace Application.Opportunities.Get;

public sealed record GetOpportunitiesQuery : IQuery<List<OpportunityResponse>>;
 