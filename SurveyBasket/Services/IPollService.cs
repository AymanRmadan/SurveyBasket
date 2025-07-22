using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;

namespace SurveyBasket.Services
{
    public interface IPollService
    {
        Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellation = default);

        //this method will return Currently Polls available
        Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellation = default);
        Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellation = default);
        Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellation = default);
        Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellation = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellation = default);
        Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellation = default);



    }
}


