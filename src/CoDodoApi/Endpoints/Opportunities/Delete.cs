using Application.Abstractions.Messaging;
using Application.Opportunities.Delete;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using SharedKernel;

namespace CoDodoApi.Endpoints.Opportunities;

public class Delete : IEndpoint
{
  public void MapEndpoint(IEndpointRouteBuilder app)
  {
    app.MapDelete("opportunity/{id:guid}", async (
        Guid id,
        ICommandHandler<DeleteOpportunityCommand> handler,
        CancellationToken cancellationToken) =>
      {
        var command = new DeleteOpportunityCommand(id);
        
        Result result = await handler.Handle(command, cancellationToken);

        return result.Match(Results.NoContent, CustomResults.Problem);
      })
      .RequireAuthorization()
      .WithOpenApi()
      .WithName(Names.Opportunities.Delete)
      .WithTags(Tags.Opportunities);
  }
}