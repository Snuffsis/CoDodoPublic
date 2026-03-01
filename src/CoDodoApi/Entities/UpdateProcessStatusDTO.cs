namespace CoDodoApi.Entities;

public sealed class UpdateProcessStatusDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment {get; set;} = "";
    public string Status {get; set;} = "";
}