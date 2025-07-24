
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Authantication;
using SurveyBasket.Contracts.Authentications.Auth.Responses;
using SurveyBasket.Contracts.Questions.Responses;
using SurveyBasket.Errors;
using System.Security.Cryptography;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpirtDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellation = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }
            var (token, expireIn) = _jwtProvider.GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirtDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration,

            });
            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName
                                   , user.LastName, token, expireIn * 60
                                   , refreshToken, refreshTokenExpiration);
            return Result.Success(response);

        }



        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var userRefrshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefrshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

            userRefrshToken.RevokedOn = DateTime.UtcNow;


            var (newToken, expireIn) = _jwtProvider.GenerateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirtDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration,

            });
            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName
                                 , user.LastName, newToken, expireIn * 60
                                 , newRefreshToken, refreshTokenExpiration);

            return Result.Success(response);
        }



        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var userRefrshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefrshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

            userRefrshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return Result.Success();
        }


        private static string GenerateRefreshToken() =>
         Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    }
}
