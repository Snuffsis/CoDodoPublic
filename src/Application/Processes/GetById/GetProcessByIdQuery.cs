using Application.Abstractions.Messaging;

namespace Application.Processes.GetById;

public sealed record GetProcessByIdQuery(
  Guid ProcessId) : IQuery<ProcessResponse>;
 