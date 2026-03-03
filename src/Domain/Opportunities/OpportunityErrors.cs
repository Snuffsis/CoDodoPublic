using SharedKernel;

namespace Domain.Opportunities;

public static class OpportunityErrors
{
  public static Error NotFound(Guid opportunityId) => Error.NotFound(
    "Opportunities.NotFound",
    $"The opportunity with the Id = '{opportunityId}' was not found");
  
  public static Error UriNotUnique(string uriForAssignment) => Error.Conflict(
    "Opportunities.UriNotUnique",
    $"The URI '{uriForAssignment}' was not unique");

  public static Error Unauthorized() => Error.Failure(
    "Opportunities.Unauthorized",
    "You are not authorized to perform this action.");
}