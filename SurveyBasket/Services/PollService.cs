
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{
    public class PollService(AppDbContext context) : IPollService
    {
        private static readonly List<Poll> _polls =
          [
          new Poll
            {
                Id = 1,
                Title="Poll 1",
                Summary="My first Poll"
            },
              new Poll
            {
                Id = 2,
                Title="Poll 2",
                Summary="My second Poll"
            }

          ];
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync() =>
            await _context.polls.AsNoTracking().ToListAsync();
        public async Task<Poll?> GetAsync(int id) =>
            await _context.polls.FindAsync(id);

        public async Task<Poll> AddAsync(Poll poll)
        {
            await _context.AddAsync(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public bool Update(int id, Poll poll)
        {
            var currentPoll = GetAsync(id);
            if (currentPoll is null)
                return false;

            /* currentPoll.Title = poll.Title;
             currentPoll.Summary = poll.Summary;*/
            return true;
        }

        public bool Delete(int id)
        {
            var poll = GetAsync(id);
            if (poll is null) return false;

            //  _polls.Remove(poll);
            return true;
        }
    }
}
