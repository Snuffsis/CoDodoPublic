using CoDodoApi.Database;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Endpoints.Processes;

/// <summary>
/// Represents the GET endpoint for microsoft Products.
/// </summary>
public class Get : IEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for microsoft Products.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("processes", async (
            ApplicationDbContext dbContext,
            ILogger<Get> logger
            ) =>
        {
            try
            {
                var r = await dbContext.Processes.Include(p => p.Opportunity).ToListAsync();

                return Results.Ok(r);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception in {Endpoint}: {ExMessage}", "GetById", ex.Message);
                return Results.BadRequest();
            }
        })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(Names.Processes.Get)
            .WithTags(Tags.Processes);
    }
}