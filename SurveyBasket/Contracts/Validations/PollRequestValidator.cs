using FluentValidation;
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class PollRequestValidator : AbstractValidator<PollRequest>
    {
        public PollRequestValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Please add a {PropertyName}")
                .Length(3, 50);

            RuleFor(r => r.Summary)
              .NotEmpty().WithMessage("Please add a {PropertyName}")
              .Length(3, 1500);

            RuleFor(r => r.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(r => r.EndsAt)
               .NotEmpty();

            //r => r ==> this check compoletly model
            RuleFor(r => r)
                .Must(HasValidDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} must be greater than or equal Starts Date");

        }

        private bool HasValidDate(PollRequest request)
        { return request.EndsAt >= request.StartsAt; }
    }
}
