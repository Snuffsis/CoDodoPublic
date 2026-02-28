using CoDodoApi.Database.Entities;

namespace CoDodoApi.Entities;

public sealed class DeleteProcessDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment { get; set; } = "";
}

public static class DeleteProcessDtoExtensions
{
    public static 
    Process ToProcess(this DeleteProcessDTO dto)
    {
        Opportunity details = Opportunity.Create(
            "",
            "",
            "",
            "",
            0);

        return Process.Create(
            dto.Name, 
            details, 
            details.UriForAssignment,
            "",
            DateTime.UtcNow,
            DateTime.UtcNow);
    }
}