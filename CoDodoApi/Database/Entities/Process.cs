using CoDodoApi.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoDodoApi.Database.Entities;

public class Process
{
    private Process(string name,
                   Opportunity opportunity,
                   string opportunityUri,
                   string status,
                   DateTime createdDate,
                   DateTime updatedDate)
    {
        Name = name;
        Opportunity = opportunity;
        OpportunityUri = opportunityUri;
        Status = status;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
    
    public Process()
    {
    }
    
    public string Name { get; set; } = "";
    public string OpportunityUri { get; set; } = "";
    public Opportunity Opportunity { get; set; } = null!;
    public string Status { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    [NotMapped]
    public TimeProvider TimeProvider { get; set; }

    public static Process Create(
        string name,
        Opportunity opportunity,
        string opportunityUri,
        string status,
        DateTime createdDate, 
        DateTime updatedDate)
    {
        var process = new Process(
            name,
            opportunity,
            opportunityUri,
            status,
            createdDate,
            updatedDate);
        return process;
    }

    public int DaysSinceUpdate()
    {
        TimeSpan diff = TimeProvider.GetUtcNow() - UpdatedDate;

        return NumberOfWholeDays(diff);
    }

    public int DaysSinceCreation()
    {
        TimeSpan diff = TimeProvider.GetUtcNow() - CreatedDate;

        return NumberOfWholeDays(diff);
    }

    static int NumberOfWholeDays(TimeSpan diff)
    {
        double numberOfDays = diff.TotalDays;

        return (int)numberOfDays;
    }

    internal bool IsWon()
    {
        return Status == "WON";
    }
}

internal static class ProcessExtensions
{
    public static ProcessDTO ToDto(this Process process)
    {
        Process p = process;
        Opportunity d = p.Opportunity;

        return new ProcessDTO(p.Name,
                              d.UriForAssignment,
                              d.Company,
                              d.Capability,
                              p.Status,
                              d.NameOfSalesLead,
                              d.HourlyRateInSEK,
                              p.UpdatedDate,
                              p.CreatedDate,
                              p.DaysSinceUpdate(),
                              p.DaysSinceCreation());
    }
}