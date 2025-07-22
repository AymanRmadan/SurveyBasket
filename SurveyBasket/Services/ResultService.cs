using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Result;
using SurveyBasket.Contracts.Result.VotesPerQuestion;
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


        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
        {

            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);
            }

            var votesPerDay = await _context.Votes.Where(vote => vote.PollId == pollId)
                     .GroupBy(vote => new { VoteDate = DateOnly.FromDateTime(vote.SubmittedOn) })
                     .Select(group => new VotesPerDayResponse(
                            group.Key.VoteDate,
                            group.Count()
                         )).ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);


        }

        public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
        {

            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);
            }

            var votesPerQuestion = await _context.VoteAnswers.Where(va => va.Vote.PollId == pollId)
                     .Select(va => new VotesPerQuestionResponse(
                         va.Question.Content,
                         va.Question.Votes.GroupBy(x => new { AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content })
                            .Select(group => new VotesPerAnswerResponse(
                                group.Key.AnswerContent,
                                group.Count()
                                ))
                            )).ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);


        }

    }
}
