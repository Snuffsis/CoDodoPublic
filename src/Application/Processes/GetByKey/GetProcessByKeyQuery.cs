using Application.Abstractions.Messaging;

namespace Application.Processes.GetByKey;

public sealed record GetProcessByKeyQuery(
  string Name,
  string UriForAssignment) : IQuery<ProcessResponse>;
 