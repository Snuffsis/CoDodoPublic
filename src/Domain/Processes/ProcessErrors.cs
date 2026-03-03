using SharedKernel;

namespace Domain.Processes;

public static class ProcessErrors
{
  public static Error NotFound(Guid processId) => Error.NotFound(
    "Processes.NotFound",
    $"The process with the Id = '{processId}' was not found");

  public static Error Unauthorized() => Error.Failure(
    "Processes.Unauthorized",
    "You are not authorized to perform this action.");
  
  public static Error KeyNotUnique() => Error.Conflict(
    "Processes.KeyNotUnique",
    "The specified key was not unique");

  public static readonly Error NotFoundByKey = Error.NotFound(
    "Processes.NotFoundByKey",
    "The process with the specified key was not found");
}