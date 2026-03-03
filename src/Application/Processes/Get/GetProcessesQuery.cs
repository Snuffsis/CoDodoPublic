using Application.Abstractions.Messaging;

namespace Application.Processes.Get;

public sealed record GetProcessesQuery : IQuery<List<ProcessResponse>>;
 