using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Services.Votes
{
    public interface IVoteServics
    {
        //Add Vote in DB
        Task<Result> AddVoteAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default);
    }
}
