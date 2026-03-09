namespace CoDodoApi.Endpoints;

public static class Names
{

    public static class Processes
    {
        public const string Get = "GetProcesses";
        public const string GetById = "GetProcessById";
        public const string GetByKey = "GetProcessByKey";
        public const string Update = "UpdateProcess";
        public const string Create = "CreateProcess";
        public const string Delete = "DeleteProcess";
    }

    public static class Opportunities
    {
      public const string Get = "GetOpportunities";
      public const string GetById = "GetOpportunityById";
      public const string Update = "UpdateOpportunity";
      public const string Create = "CreateOpportunity";
      public const string Delete = "DeleteOpportunity";
    }
    
    public static class Imports
    {
        public const string Import = "ImportProcesses";
    }
}