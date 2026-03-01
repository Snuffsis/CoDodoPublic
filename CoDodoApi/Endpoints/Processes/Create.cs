using CoDodoApi.Database;
using CoDodoApi.Database.Entities;
using CoDodoApi.Entities;

namespace CoDodoApi.Endpoints.Processes;

/// <summary>
/// Represents the GET endpoint for microsoft Products.
/// </summary>
public class Create : IEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for microsoft Products.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("processes", async (
                CreateProcessDTO dto,
                ILogger<Create> logger,
                ApplicationDbContext dbContext
            ) =>
            {
                try
                {
                    Process process = dto.ToProcess();
            
                    await dbContext.Processes.AddAsync(process);

                    await dbContext.SaveChangesAsync();

                    return Results.Created($"/processes/{process.OpportunityUri}", process);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception thrown while trying to create a process");
                    return Results.BadRequest();
                }
            })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName("Create a Process")
            .WithTags("Processes");
    }
}