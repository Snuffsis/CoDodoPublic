using CoDodoApi.Database;
using CoDodoApi.Services;

namespace CoDodoApi.Endpoints.Import;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("import", async (
            IFormFile file,
            ILogger<Create> logger,
            ApplicationDbContext dbContext,
            ExcelService excelService
        ) =>
        {
            try
            {
                var processes = await excelService.ExtractProcesses(file);
                
                if (processes.Count == 0)
                {
                    return Results.Ok();
                }

                try
                {
                    dbContext.Processes.AddRange(processes);
                  
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured while importing excel file. {Error}", ex.Message);
                    return Results.Problem("Failed to import excel file.");
                }
                return Results.Ok();
            }
            catch(Exception ex)
            {
                logger.LogError(ex,  "An error occured while processing excel file. {Error}", ex.Message);
                return Results.Problem("Failed to process excel file.");
            }
        })
        .DisableAntiforgery()
        .RequireAuthorization()
        .WithOpenApi()
        .WithTags(Tags.Imports)
        .WithName(Names.Imports.Import);
    }
}