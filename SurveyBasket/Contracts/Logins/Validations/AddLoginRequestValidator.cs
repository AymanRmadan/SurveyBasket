using FluentValidation;
using SurveyBasket.Contracts.Logins.Request;

namespace SurveyBasket.Contracts.Validations
{
    public class AddLoginRequestValidator : AbstractValidator<AddLoginRequest>
    {
        public AddLoginRequestValidator()
        {
            RuleFor(r => r.email).NotEmpty().EmailAddress();
            RuleFor(r => r.password).NotEmpty();


        }

    }
}
