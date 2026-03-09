using Application.Abstractions.Messaging;
using Application.Processes.Get;
using CoDodoApi.Database;
using CoDodoApi.Extensions;
using CoDodoApi.Infrastructure;
using Domain.Processes;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace CoDodoApi.Endpoints.Processes;

/// <summary>
/// Represents the GET endpoint for processes.
/// </summary>
public class Get : IEndpoint
{
  /// <summary>
  /// Maps the GET endpoint for processes.
  /// </summary>
  /// <param name="app">The endpoint route builder.</param>
  public void MapEndpoint(IEndpointRouteBuilder app)
  {
    app.MapGet("processes", async (
        IQueryHandler<GetProcessesQuery, List<ProcessResponse>> handler,
        CancellationToken cancellationToken) =>
      {
        var query = new GetProcessesQuery();
        
        Result<List<ProcessResponse>> result = await handler.Handle(query, cancellationToken);

        return result.Match(Results.Ok, CustomResults.Problem);
      })
      .RequireAuthorization()
      .WithOpenApi()
      .WithName(Names.Processes.Get)
      .WithTags(Tags.Processes);
  }
}