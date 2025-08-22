using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Abstractions.Consts;
using SurveyBasket.Contracts.Users;
using SurveyBasket.Contracts.Users.Requests;
using SurveyBasket.Contracts.Users.Responses;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{

    public class UserService(UserManager<ApplicationUser> userManager, AppDbContext context) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await (from u in _context.Users
               join ur in _context.UserRoles
               on u.Id equals ur.UserId
               join r in _context.Roles
               on ur.RoleId equals r.Id into roles
               where !roles.Any(x => x.Name == DefaultRoles.Member)
               select new
               {
                   u.Id,
                   u.FirstName,
                   u.LastName,
                   u.Email,
                   u.IsDisabled,
                   Roles = roles.Select(x => x.Name!).ToList()
               }
            )
            .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })
            .Select(u => new UserResponse
            (
                u.Key.Id,
                u.Key.FirstName,
                u.Key.LastName,
                u.Key.Email,
                u.Key.IsDisabled,
                u.SelectMany(x => x.Roles)
            ))
           .ToListAsync(cancellationToken);
        public async Task<Result<UserResponse>> GetAsync(string id)
        {
            if (await _userManager.FindByIdAsync(id) is not { } user)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userRoles = await _userManager.GetRolesAsync(user);

            var response = (user, userRoles).Adapt<UserResponse>();

            return Result.Success(response);
        }
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
            //var user = await _userManager.FindByIdAsync(userId);

            //user = request.Adapt(user);

            //await _userManager.UpdateAsync(user!);

            await _userManager.Users
                .Where(x => x.Id == userId)
                .ExecuteUpdateAsync(setters =>
                    setters
                        .SetProperty(x => x.FirstName, request.FirstName)
                        .SetProperty(x => x.LastName, request.LastName)
                );

            return Result.Success();

        }

        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}
