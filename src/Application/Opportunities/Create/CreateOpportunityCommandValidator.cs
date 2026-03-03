using FluentValidation;

namespace Application.Opportunities.Create;

public class CreateOpportunityCommandValidator : AbstractValidator<CreateOpportunityCommand>
{
  public CreateOpportunityCommandValidator()
  {
    RuleFor(c => c.UriForAssignment).NotEmpty();
  }
}