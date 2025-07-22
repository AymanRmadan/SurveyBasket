using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Questions.Responses;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{
    public class VoteService(AppDbContext context) : IVoteServics
    {
        private readonly AppDbContext _context = context;

        public async Task<Result> AddVoteAsync(int pollId, string userId, VoteRequest request, CancellationToken cancellationToken = default)
        {
            // Do User vote on this poll or no
            var hasVote = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken: cancellationToken);
            if (hasVote)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);
            }

            // Do User vote on this poll or no
            var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken: cancellationToken);
            if (!pollIsExist)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            }

            // check if Questions in request is the same question in poll
            var availableQuestions = await _context.Questions
                                          .Where(q => q.PollId == pollId && q.IsActive)
                                          .Select(q => q.Id)
                                          .ToListAsync(cancellationToken);
            // compare questions Ids in requst == question Ids in availableQuestions
            if (!request.Answers.Select(a => a.QuestionId).SequenceEqual(availableQuestions))
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.InvalidQuestions);
            }

            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                VoteAnswers = request.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList(),
            };

            await _context.Votes.AddAsync(vote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
