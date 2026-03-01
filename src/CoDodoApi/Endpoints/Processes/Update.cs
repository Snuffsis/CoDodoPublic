using CoDodoApi.Database;
using CoDodoApi.Entities;

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
                UpdateProcessStatusDTO dto,
                ILogger<Update> logger,
                ApplicationDbContext dbContext
            ) =>
            {
                try
                {
                    var process = await dbContext.Processes.FindAsync(dto.Name, dto.UriForAssignment);
                    if (process == null)
                    {
                        return Results.NotFound();
                    }
                    
                    process.Status = dto.Status;
                    process.UpdatedDate = DateTime.UtcNow;
            
                    dbContext.Processes.Update(process);

                    await dbContext.SaveChangesAsync();

                    return Results.Ok(process);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception thrown while trying to create a process");
                    return Results.BadRequest();
                }
            })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(Names.Processes.Update)
            .WithTags(Tags.Processes);
    }
}