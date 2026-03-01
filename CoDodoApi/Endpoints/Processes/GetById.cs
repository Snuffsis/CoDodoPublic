using CoDodoApi.Database;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Endpoints.Processes;

/// <summary>
/// Represents the GET endpoint for microsoft Products.
/// </summary>
public class GetById : IEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for microsoft Products.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("processes/getByKey", async (
                string name,
                string opportunityUri,
                ApplicationDbContext dbContext,
                ILogger<GetById> logger
            ) =>
            {
                try
                {
                    var r = await dbContext.Processes
                        .Include(p => p.Opportunity)
                        .FirstOrDefaultAsync(p => 
                            p.Name == name && 
                            p.OpportunityUri == opportunityUri);

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
            .WithName(Names.Processes.GetByKey)
            .WithTags(Tags.Processes);
    }
}