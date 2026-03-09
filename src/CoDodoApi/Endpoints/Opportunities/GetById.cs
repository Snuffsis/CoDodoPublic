using Application.Abstractions.Messaging;
using Application.Opportunities.GetById;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using SharedKernel;

namespace CoDodoApi.Endpoints.Opportunities;

/// <summary>
/// Represents the GET endpoint for opportunities.
/// </summary>
public class GetById : IEndpoint
{
  /// <summary>
  /// Maps the GET endpoint for opportunities.
  /// </summary>
  /// <param name="app">The endpoint route builder.</param>
  public void MapEndpoint(IEndpointRouteBuilder app)
  {
    app.MapGet("opportunities/{id:guid}", async (
        Guid id,
        IQueryHandler<GetOpportunityByIdQuery, OpportunityResponse> handler,
        CancellationToken cancellationToken
      ) =>
      {
        var query = new GetOpportunityByIdQuery(id);
              
        Result<OpportunityResponse> result = await handler.Handle(query, cancellationToken);
              
        return result.Match(Results.Ok, CustomResults.Problem);
      })
      .RequireAuthorization()
      .WithOpenApi()
      .WithName(Names.Opportunities.GetById)
      .WithTags(Tags.Opportunities);
  }
}