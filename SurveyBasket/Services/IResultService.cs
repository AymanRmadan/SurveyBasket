using SurveyBasket.Contracts.Result;

namespace SurveyBasket.Services
{
    public interface IResultService
    {
        Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default);

    }
}
