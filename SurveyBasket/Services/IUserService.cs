using SurveyBasket.Contracts.Users.Requests;
using SurveyBasket.Contracts.Users.Responses;

namespace SurveyBasket.Services
{
    public interface IUserService
    {
        public Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
        public Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    }
}
