using FluentValidation;

namespace Application.Processes.Update;

internal sealed class UpdateProcessCommandValidator : AbstractValidator<UpdateProcessCommand>
{
    public UpdateProcessCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.OpportunityId).NotEmpty();
    }
}
