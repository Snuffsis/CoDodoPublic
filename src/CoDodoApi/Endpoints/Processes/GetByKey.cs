using Application.Abstractions.Messaging;
using Application.Processes.GetByKey;
using CoDodoApi.Database;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace CoDodoApi.Endpoints.Processes;

/// <summary>
/// Represents the GET endpoint for fetching processes by its key.
/// </summary>
public class GetByKey : IEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for fetching processes by its key.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("processes/getByKey", async (
            string name,
            string uriForAssignment,
            IQueryHandler<GetProcessByKeyQuery, ProcessResponse> handler,
            CancellationToken cancellationToken
            ) =>
            {
              var query = new GetProcessByKeyQuery(name, uriForAssignment);
              
              Result<ProcessResponse> result = await handler.Handle(query, cancellationToken);
              
              return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(Names.Processes.GetByKey)
            .WithTags(Tags.Processes);
    }
}