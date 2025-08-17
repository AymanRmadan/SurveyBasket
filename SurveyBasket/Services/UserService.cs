using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Users.Requests;
using SurveyBasket.Contracts.Users.Responses;

namespace SurveyBasket.Services
{

    public class UserService(UserManager<ApplicationUser> userManager) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            var user = await _userManager.Users
                .Where(u => u.Id == userId)
                .ProjectToType<UserProfileResponse>()
                .SingleAsync();

            //return Result.Success(user.Adapt<UserProfileResponse>());
            return Result.Success(user);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            user = request.Adapt(user);

            await _userManager.UpdateAsync(user!);
            return Result.Success();

        }
    }
}
