using Application.Abstractions.Messaging;
using Application.Processes.Update;
using CoDodoApi.Database;
using CoDodoApi.Entities;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using Domain.Processes;
using SharedKernel;

namespace CoDodoApi.Endpoints.Processes;

/// <summary>
/// Represents the PUT endpoint for updating a processing.
/// </summary>
public class Update : IEndpoint
{
    /// <summary>
    /// Maps the PUT endpoint for updating a processing.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("processes", async (
            string name,
            string uriForAssignment,
            Status status,
            ICommandHandler<UpdateProcessCommand> handler,
            CancellationToken cancellationToken) =>
            {
              var command = new UpdateProcessCommand(name, uriForAssignment, status);
              
              Result result = await handler.Handle(command, cancellationToken);

              return result.Match(Results.NoContent, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(Names.Processes.Update)
            .WithTags(Tags.Processes);
    }
}