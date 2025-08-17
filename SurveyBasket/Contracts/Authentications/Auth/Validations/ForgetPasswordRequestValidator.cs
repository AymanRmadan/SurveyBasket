using FluentValidation;
using SurveyBasket.Contracts.Authentications.Auth.Requests;

namespace SurveyBasket.Contracts.Authentications.Auth.Validations
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .EmailAddress();


        }
    }
}
