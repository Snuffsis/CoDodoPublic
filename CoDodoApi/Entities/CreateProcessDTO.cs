using CoDodoApi.Database.Entities;

namespace CoDodoApi.Entities;

public sealed class CreateProcessDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment {get; set;} = "";
    public string Company {get; set;} = "";
    public string Capability {get; set;} = "";
    public string Opportunity {get; set;} = "";
    public string Status {get; set;} = "";
    public string NameOfSalesLead {get; set;} = "";
    public int HourlyRateInSEK {get; set;}
    public string Notes { get; set; } = "";
}

public static class CreateProcessDtoExtensions
{
    public static Process ToProcess(this CreateProcessDTO dto)
    {
        Opportunity o = Opportunity.Create(
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
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}