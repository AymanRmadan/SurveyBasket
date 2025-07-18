using FluentValidation;
using SurveyBasket.Contracts.Authentications.Requests;

namespace SurveyBasket.Contracts.Authentications.Validations
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