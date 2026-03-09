using Application.Abstractions.Messaging;
using Application.Opportunities.Create;
using Application.Processes.Create;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using SharedKernel;

namespace CoDodoApi.Endpoints.Opportunities;

/// <summary>
/// Represents the POST endpoint for creating a new opportunity.
/// </summary>
public class Create : IEndpoint
{
  /// <summary>
  /// Maps the POST endpoint for creating a new opportunity.
  /// </summary>
  /// <param name="app">The endpoint route builder.</param>
  public void MapEndpoint(IEndpointRouteBuilder app)
  {
    app.MapPost("opportunities", async (
        CreateOpportunityCommand request,
        ICommandHandler<CreateOpportunityCommand, Guid> handler,
        CancellationToken cancellationToken
      ) =>
      {
        Result<Guid> result = await handler.Handle(request, cancellationToken);

        return result.Match(Results.Ok, CustomResults.Problem);
      })
      .RequireAuthorization()
      .WithOpenApi()
      .WithName(Names.Opportunities.Create)
      .WithTags(Tags.Opportunities);
  }
}