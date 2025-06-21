
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{
    public class PollService(AppDbContext context) : IPollService
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellation) =>
            await _context.polls.AsNoTracking().ToListAsync(cancellation);
        public async Task<Poll?> GetAsync(int id, CancellationToken cancellation = default) =>
            await _context.polls.FindAsync(id, cancellation);


        /*CancellationToken this is use when i added items and wanted to cancel this item this CancellationToken help to cancel and not saved in database */
        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellation)
        {
            await _context.AddAsync(poll, cancellation);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellation)
        {
            var currentPoll = await GetAsync(id, cancellation);
            if (currentPoll is null)
                return false;

            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.IsPublished = poll.IsPublished;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;

            await _context.SaveChangesAsync(cancellation);


            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellation)
        {
            var poll = await GetAsync(id, cancellation);
            if (poll is null) return false;

            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellation);
            return true;
        }

        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellation = default)
        {
            var poll = await GetAsync(id, cancellation);
            if (poll is null)
                return false;

            poll.IsPublished = !poll.IsPublished;

            await _context.SaveChangesAsync(cancellation);


            return true;
        }
    }
}
