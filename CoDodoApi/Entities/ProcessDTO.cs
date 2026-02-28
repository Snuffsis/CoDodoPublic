using CoDodoApi.Database.Entities;

namespace CoDodoApi.Entities;

public sealed
class ProcessDTO
{
    public ProcessDTO(string name,
                      string uriForAssignment,
                      string company,
                      string capability,
                      string status,
                      string nameOfSalesLead,
                      int hourlyRateInSEK,
                      DateTime updatedDate,
                      DateTime createdDate,
                      int daysSinceUpdate,
                      int daysSinceCreation)
    {
        Name = name;
        UriForAssignment = uriForAssignment;
        Company = company;
        Capability = capability;
        Status = status;
        NameOfSalesLead = nameOfSalesLead;
        HourlyRateInSEK = hourlyRateInSEK;
        UpdatedDate = updatedDate;
        CreatedDate = createdDate;
        DaysSinceUpdate = daysSinceUpdate;
        DaysSinceCreation = daysSinceCreation;
    }
    public string Name { get; set; } = "";
    public string UriForAssignment { get; set; } = "";
    public string Company { get; set; } = "";
    public string Capability { get; set; } = "";
    public string Status { get; set; } = "";
    public string NameOfSalesLead { get; set; } = ""; // Sales Lead
    public int HourlyRateInSEK { get; set; } // Hourly Rate
    public DateTime UpdatedDate { get; set; }  // Last Update
    public DateTime CreatedDate { get; set; } // Generation Date
    public int DaysSinceUpdate { get; set; }// Freshness
    public int DaysSinceCreation { get; set; } // Age
}

public static class ProcessDtoExtensions
{
    public static Process ToProcess(this ProcessDTO dto, TimeProvider provider)
    {
        Opportunity o =
            Opportunity.Create(
                dto.UriForAssignment,
                dto.Company,
                dto.Capability,
                dto.NameOfSalesLead,
                dto.HourlyRateInSEK);
        
        return Process.Create(
            dto.Name,
            o,
            o.UriForAssignment,
            dto.Status,
            dto.CreatedDate,
            dto.UpdatedDate);
    }
}