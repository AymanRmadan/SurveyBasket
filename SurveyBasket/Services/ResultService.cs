using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Result;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{
    public class ResultService(AppDbContext context) : IResultService
    {
        private readonly AppDbContext _context = context;

        public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollVotes = await _context.Polls.Where(poll => poll.Id == pollId)
                            .Select(poll => new PollVotesResponse(
                                poll.Title,
                                poll.Votes.Select(vote => new VoteResponse(
                                    $"{vote.User.FirstName} {vote.User.LastName}",
                                             vote.SubmittedOn,
                                             vote.VoteAnswers.Select(answer => new QuestionAnswerResponse(
                                                 answer.Question.Content,
                                                 answer.Answer.Content
                                                 ))
                                    )))).SingleOrDefaultAsync(cancellationToken);
            return pollVotes is null
                 ? Result.Failure<PollVotesResponse>(PollErrors.PollNotFound)
                 : Result.Success(pollVotes);
        }
    }
}
