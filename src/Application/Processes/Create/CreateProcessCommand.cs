using Application.Abstractions.Messaging;
using Domain.Processes;

namespace Application.Processes.Create;

public sealed record CreateProcessCommand(
  string Name,
  string UriForAssignment,
  Status Status
  ) : ICommand<Guid>;