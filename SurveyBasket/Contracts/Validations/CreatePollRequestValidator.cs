using FluentValidation;
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
    {
        public CreatePollRequestValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Please add a {PropertyName}")
                .Length(3, 50);
        }
    }
}
