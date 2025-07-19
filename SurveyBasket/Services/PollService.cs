
using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{
    public class PollService(AppDbContext context) : IPollService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellation) =>
            await _context.polls.AsNoTracking().ToListAsync(cancellation);
        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellation = default)
        {
            var poll = await _context.polls.FindAsync(id, cancellation);

            return poll is not null ? Result.Success(poll.Adapt<PollResponse>()) : Result.Failure<PollResponse>(PollErrors.PollNotFound);
        }


        /*CancellationToken this is use when i added items and wanted to cancel this item this CancellationToken help to cancel and not saved in database */
        public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellation)
        {
            var isExistintTitle = await _context.polls.AnyAsync(x => x.Title == request.Title, cancellationToken: cancellation);
            if (isExistintTitle)
            {
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);
            }
            var poll = request.Adapt<Poll>();
            await _context.AddAsync(poll, cancellation);
            await _context.SaveChangesAsync();
            return Result.Success(poll.Adapt<PollResponse>());
        }

        public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellation)
        {
            var isExistintTitle = await _context.polls.AnyAsync(x => x.Title == request.Title && x.Id != id, cancellationToken: cancellation);
            if (isExistintTitle)
            {
                return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);
            }
            //var currentPoll = await GetAsync(id, cancellation);
            var currentPoll = await _context.polls.FindAsync(id, cancellation);
            if (currentPoll is null)
                return Result.Failure(PollErrors.PollNotFound);

            currentPoll.Title = request.Title;
            currentPoll.Summary = request.Summary;
            currentPoll.IsPublished = request.IsPublished;
            currentPoll.StartsAt = request.StartsAt;
            currentPoll.EndsAt = request.EndsAt;

            await _context.SaveChangesAsync(cancellation);


            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellation)
        {
            var poll = await _context.polls.FindAsync(id, cancellation);
            if (poll is null) return Result.Failure(PollErrors.PollNotFound);

            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellation);
            return Result.Success();
        }

        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellation = default)
        {
            // var poll = await GetAsync(id, cancellation);
            var poll = await _context.polls.FindAsync(id, cancellation);
            if (poll is null)
                return Result.Failure(PollErrors.PollNotFound);

            poll.IsPublished = !poll.IsPublished;

            await _context.SaveChangesAsync(cancellation);


            return Result.Success();
        }
    }
}
