using Application.Abstractions.Messaging;
using Application.Opportunities.Get;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using SharedKernel;

namespace CoDodoApi.Endpoints.Opportunities;

/// <summary>
/// Represents the GET endpoint for opportunities.
/// </summary>
public class Get : IEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for opportunities.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("opportunities", async (
            IQueryHandler<GetOpportunitiesQuery, List<OpportunityResponse>> handler,
            CancellationToken cancellationToken
            ) =>
            {
              var query = new GetOpportunitiesQuery();
              
              Result<List<OpportunityResponse>> result = await handler.Handle(query, cancellationToken);
              
              return result.Match(Results.Ok, CustomResults.Problem);
        })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(Names.Opportunities.Get)
            .WithTags(Tags.Opportunities);
    }
}