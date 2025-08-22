using SurveyBasket.Contracts.Users;
using SurveyBasket.Contracts.Users.Requests;
using SurveyBasket.Contracts.Users.Responses;

namespace SurveyBasket.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetAsync(string id);

        Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> ToggleStatus(string id);
        Task<Result> Unlock(string id);
        public Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
        public Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);

        public Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }
}
