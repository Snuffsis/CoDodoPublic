namespace CoDodoApi.Database.Entities;

public class Opportunity
{
    private Opportunity(string uriForAssignment,
                       string company,
                       string capability,
                       string nameOfSalesLead,
                       int hourlyRateInSEK)
    {
        UriForAssignment = uriForAssignment;
        Company = company;
        Capability = capability;
        NameOfSalesLead = nameOfSalesLead;
        HourlyRateInSEK = hourlyRateInSEK;
    }
    
    public Opportunity() { }
    
    
    public string UriForAssignment { get; set; } = "";
    public string Company { get; set; } = "";
    public string Capability { get; set; } = "";
    public string NameOfSalesLead { get; set; } = "";
    public int HourlyRateInSEK { get; set; }

    public static Opportunity Create(
        string uriForAssignment,
        string company,
        string capability,
        string nameOfSalesLead,
        int hourlyRateInSEK)
    {
        var opportunity = new Opportunity(
            uriForAssignment,
            company,
            capability,
            nameOfSalesLead,
            hourlyRateInSEK);
        return opportunity;
    }
}