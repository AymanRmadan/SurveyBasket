using SurveyBasket.Contracts.Authentications.Emails;
using SurveyBasket.Contracts.Authentications.Register;
using SurveyBasket.Contracts.Authentications.ResentConfirmationEmail;

namespace SurveyBasket.Services.Authentication
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellation = default);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default);
        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default);

        Task<Result> RegisterAsync(AddRegisterRequest request, CancellationToken cancellationToken = default);


        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);

        Task<Result> ResendConfirmationEmailAsync(AddResendConfirmationEmailRequest request);


        Task<Result> SendResetPasswordCodeAsync(string email);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);


    }
}
