
using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<AuthResponse> GetTokenAsync(string email, string password, CancellationToken cancellation = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return null;
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
            {
                return null;
            }

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, "asaksjkajdakndadadknakdlandlandladlda", 3600);

        }
    }
}
