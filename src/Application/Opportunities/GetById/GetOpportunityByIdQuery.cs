using Application.Abstractions.Messaging;

namespace Application.Opportunities.GetById;

public sealed record GetOpportunityByIdQuery(
  Guid OpportunityId) : IQuery<OpportunityResponse>;
 