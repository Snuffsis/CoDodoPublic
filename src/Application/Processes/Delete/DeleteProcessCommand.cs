using Application.Abstractions.Messaging;

namespace Application.Processes.Delete;

public sealed record DeleteProcessCommand(Guid ProcessId) : ICommand;
