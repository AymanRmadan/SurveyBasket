using FluentValidation;
using SurveyBasket.Contracts.Authentications.Auth.Requests;

namespace SurveyBasket.Contracts.Authentications.Auth.Validations
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(r => r.token).NotEmpty();
            RuleFor(r => r.refreshToken).NotEmpty();


        }

    }
}