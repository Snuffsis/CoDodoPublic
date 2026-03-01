using CoDodoApi.Database;
using CoDodoApi.Database.Entities;
using CoDodoApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CoDodoApi.Endpoints.Processes;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("process", async (
                    [FromBody] DeleteProcessDTO dto,
                    ApplicationDbContext dbContext,
                    ILogger<Delete> logger)
                =>
            {
                try
                {
                    Process process = dto.ToProcess();

                    var r = dbContext.Processes.Remove(process);
                    await dbContext.SaveChangesAsync();
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception in {Endpoint}: {ExMessage}", "Delete", ex.Message);
                    return Results.BadRequest();
                }
            })
            .RequireAuthorization()
            .WithOpenApi()
            .WithName(Names.Processes.Delete)
            .WithTags(Tags.Processes);
    }
}
