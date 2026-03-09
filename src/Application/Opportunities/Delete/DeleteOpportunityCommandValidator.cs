using FluentValidation;

namespace Application.Opportunities.Delete;

internal sealed class DeleteOpportunityCommandValidator : AbstractValidator<DeleteOpportunityCommand>
{
    public DeleteOpportunityCommandValidator()
    {
        RuleFor(c => c.OpportunityId).NotEmpty();
    }
}
