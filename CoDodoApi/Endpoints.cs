using CoDodoApi.Database;
using CoDodoApi.Database.Entities;
using CoDodoApi.Entities;
using CoDodoApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi;

static class Endpoints
{
    public static async
    Task<IResult> DeleteProcess([FromBody] DeleteProcessDTO dto,
                                ApplicationDbContext dbContext,
                                TimeProvider provider,
                                ILoggerFactory logger)
    {
        try
        {
            Process process = dto.ToProcess();

            dbContext.Processes.Remove(process);
            
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return OkProcessDto(process);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(DeleteProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async
    Task<IResult> CreateProcess(CreateProcessDTO dto,
        ApplicationDbContext dbContext,
                                TimeProvider provider,
                                ILoggerFactory logger)
    {
        try
        {
            Process process = dto.ToProcess();

            dbContext.Processes.Add(process);
            
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return OkProcessDto(process);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(CreateProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async
    Task AllProcesses(ApplicationDbContext dbContext,
                      ILoggerFactory logger,
                      HttpContext context)
    {
        try
        {
            var r = await dbContext.Processes.Include(p => p.Opportunity).ToListAsync();

            context.Response.StatusCode = 200;

            await context.Response.WriteAsJsonAsync(r);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(AllProcesses)}: {ex.Message}");

            context.Response.StatusCode = 500;
        }
    }

    static IResult OkProcessDto(Process process)
    {
        ProcessDTO dto = process.ToDto();

        return TypedResults.Ok(dto);
    }

    static IResult OkProcessesDto(Process[] processes)
    {
        ProcessDTO[] dtos = processes
            .Select(p => p.ToDto())
            .ToArray();

        return TypedResults.Ok(dtos);
    }

    public static async Task<IResult> ImportExcel(IFormFile file, ExcelImporter importer)
    {
        try
        {
            await importer.Import(file);

            return Results.Ok();
        }
        catch
        {
            return Results.Problem("Failed to import excel file.");
        }
    }
}