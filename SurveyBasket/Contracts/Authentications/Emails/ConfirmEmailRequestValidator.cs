using FluentValidation;

namespace SurveyBasket.Contracts.Authentications.Emails
{
    public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
    {
        public ConfirmEmailRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotEmpty();


            RuleFor(r => r.Code)
                .NotEmpty();

        }

    }
}