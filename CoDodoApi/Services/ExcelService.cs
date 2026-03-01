using ClosedXML.Excel;
using CoDodoApi.Database.Entities;

namespace CoDodoApi.Services;

public class ExcelService
{
    private readonly ILogger<ExcelService> _logger;
    
    public ExcelService(ILogger<ExcelService> logger)
    {
        _logger = logger;
    }

    public Task<List<Process>> ExtractProcesses(IFormFile file)
    {
        try
        {
            Stream readStream = file.OpenReadStream();

            using XLWorkbook wb = new(readStream);

            IXLWorksheet ws = wb.Worksheet(1);

            IXLRows rows = ws.Rows();

            List<Process> processes = rows
                .Skip(1)
                .TakeWhile(x => !x.Cell(1).IsEmpty())
                .Select(RowToProcess)
                .ToList();

            return Task.FromResult(processes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception in {nameof(ExcelService)}");
            throw;
        }
    }

    private static Process RowToProcess(IXLRow row)
    {
        IXLCell rName = row.Cell(1);
        IXLCell rCapability = row.Cell(2);
        IXLCell rOpportunity = row.Cell(3);
        IXLCell rStatus = row.Cell(4);
        IXLCell rSalesLead = row.Cell(5);
        IXLCell rHourlyRate = row.Cell(6);
        IXLCell rLastUpdate = row.Cell(7);
        IXLCell rGenerationDate = row.Cell(8);

        string name = rName.GetValue<string>();
        string capability = rCapability.GetValue<string>();
        string company = rOpportunity.GetValue<string>();
        string status = rStatus.GetValue<string>();
        string salesLead = rSalesLead.GetValue<string>();
        rHourlyRate.TryGetValue(out int hourlyRate);
        string lu = rLastUpdate.GetValue<string>();
        string gd = rGenerationDate.GetValue<string>();

        DateTime updatedDate = DateTime.SpecifyKind(DateTime.Parse(lu), DateTimeKind.Utc);
        DateTime createdDate = DateTime.SpecifyKind(DateTime.Parse(gd), DateTimeKind.Utc);

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
            createdDate,
            updatedDate);
    }
}
