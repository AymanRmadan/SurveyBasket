using FluentValidation;
using SurveyBasket.Contracts.Logins.Request;

namespace SurveyBasket.Contracts.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(r => r.email).NotEmpty().EmailAddress();
            RuleFor(r => r.password).NotEmpty();


        }

    }
}
