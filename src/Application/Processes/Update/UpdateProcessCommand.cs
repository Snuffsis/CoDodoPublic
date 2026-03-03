using Application.Abstractions.Messaging;
using Domain.Processes;

namespace Application.Processes.Update;

public sealed record UpdateProcessCommand(
    string Name,
    string OpportunityId,
    Status? Status) : ICommand;
