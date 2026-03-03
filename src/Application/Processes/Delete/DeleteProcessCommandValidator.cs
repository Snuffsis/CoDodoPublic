using FluentValidation;

namespace Application.Processes.Delete;

internal sealed class DeleteProcessCommandValidator : AbstractValidator<DeleteProcessCommand>
{
    public DeleteProcessCommandValidator()
    {
        RuleFor(c => c.ProcessId).NotEmpty();
    }
}
