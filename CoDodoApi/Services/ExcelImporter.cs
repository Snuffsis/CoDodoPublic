using ClosedXML.Excel;
using CoDodoApi.Database;
using CoDodoApi.Database.Entities;
using CoDodoApi.Entities;
using System.Globalization;

namespace CoDodoApi.Services;

public record ExcelImporter(ApplicationDbContext DbContext,
                            TimeProvider Provider,
                            ILogger<ExcelImporter> Logger)
{
    readonly ApplicationDbContext _dbContext = DbContext;
    readonly TimeProvider timeProvider = Provider;
    readonly ILogger logger = Logger;

    public async Task Import(IFormFile file)
    {
        try
        {
            Stream readStream = file.OpenReadStream();

            using XLWorkbook wb = new(readStream);

            IXLWorksheet ws = wb.Worksheet(1);

            IXLRows rows = ws.Rows();

            IEnumerable<Process> processes = rows
                .Skip(1)
                .TakeWhile(x => !x.Cell(1).IsEmpty())
                .Select(RowToProcess);
            
            _dbContext.Processes.AddRange(processes);

            try
            {
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception in {nameof(ExcelImporter)}");
            }

        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception in {nameof(ExcelImporter)}");
            throw;
        }
    }

    Process RowToProcess(IXLRow row)
    {
        IXLCell NAME = row.Cell(1);
        IXLCell CAPABILITY = row.Cell(2);
        IXLCell OPPORTUNITY = row.Cell(3);
        IXLCell STATUS = row.Cell(4);
        IXLCell SALESLEAD = row.Cell(5);
        IXLCell HOURLYRATE = row.Cell(6);
        IXLCell LASTUPDATE = row.Cell(7);
        IXLCell GENERATIONDATE = row.Cell(8);

        string name = NAME.GetValue<string>();
        string capability = CAPABILITY.GetValue<string>();
        string company = OPPORTUNITY.GetValue<string>();
        string status = STATUS.GetValue<string>();
        string salesLead = SALESLEAD.GetValue<string>();
        HOURLYRATE.TryGetValue(out int hourlyRate);
        string lu = LASTUPDATE.GetValue<string>();
        string gd = GENERATIONDATE.GetValue<string>();

        DateTime LastUpdate = DateTime.SpecifyKind(DateTime.Parse(lu), DateTimeKind.Utc);
        DateTime generationDate = DateTime.SpecifyKind(DateTime.Parse(gd), DateTimeKind.Utc);

        string uri = Guid.NewGuid().ToString();

        Opportunity opportunity = Opportunity.Create(
            uri,
            company,
            capability,
            salesLead,
            hourlyRate);

        return Process.Create(
            name,
            opportunity,
            opportunity.UriForAssignment,
            status,
            generationDate,
            LastUpdate);
    }
}
