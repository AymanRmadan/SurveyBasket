using FluentValidation;

namespace SurveyBasket.Contracts.Authentications.ResentConfirmationEmail
{
    public class AddResendConfirmationEmailRequestValidator : AbstractValidator<AddResendConfirmationEmailRequest>
    {
        public AddResendConfirmationEmailRequestValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
