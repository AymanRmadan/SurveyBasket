using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.Contracts.Answers.Responses;
using SurveyBasket.Contracts.Common;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Contracts.Questions.Responses;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;
using System.Linq.Dynamic.Core;
namespace SurveyBasket.Services.Questions
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<QuestionService> _logger;
        private readonly HybridCache _hybridCache;
        private const string _cachPrefix = "AvailableQuestions";

        public QuestionService(AppDbContext context,
            ILogger<QuestionService> logger,
            HybridCache hybridCache)
        {
            _context = context;
            _logger = logger;
            _hybridCache = hybridCache;
        }

        public async Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(x => x.Id == pollId, cancellationToken: cancellationToken);

            if (!pollIsExists)
                return Result.Failure<PaginatedList<QuestionResponse>>(PollErrors.PollNotFound);

            var query = _context.Questions
                .Where(x => x.PollId == pollId);

            if (!string.IsNullOrEmpty(filters.SearchValue))
            {
                query = query.Where(x => x.Content.Contains(filters.SearchValue));
            }

            if (!string.IsNullOrEmpty(filters.SortColumn))
            {
                query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
            }

            var source = query
                            .Include(x => x.Answers)
                            .ProjectToType<QuestionResponse>()
                            .AsNoTracking();

            var questions = await PaginatedList<QuestionResponse>.CreateAsync(source, filters.PageNumber, filters.PageSize, cancellationToken);

            return Result.Success(questions);
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
        {
            //// Do User vote on this poll or no
            //var hasVote = await _context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken: cancellationToken);
            //if (hasVote)
            //{
            //    return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);
            //}

            //// Do User vote on this poll or no
            //var pollIsExist = await _context.Polls.AnyAsync(p => p.Id == pollId && p.IsPublished && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken: cancellationToken);
            //if (!pollIsExist)
            //{
            //    return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            //}

            #region Distributed Cache
            /*var key = $"{_cachPrefix}-{pollId}";
            var cachedQuestions = await _cacheService.GetAsync<IEnumerable<QuestionResponse>>(key, cancellationToken);
            IEnumerable<QuestionResponse> questions = [];

            if (cachedQuestions is null)
            {
                _logger.LogInformation("Get Questions from Database");

                questions = await _context.Questions
                     .Where(q => q.PollId == pollId && q.IsActive)
                     .Include(q => q.Answers)
                     .Select(q => new QuestionResponse
                     (
                         q.Id,
                         q.Content,
                         q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
                     )).AsNoTracking()
                       .ToListAsync(cancellationToken);

                await _cacheService.SetAsync(key, questions, cancellationToken);
            }
            else
            {
                _logger.LogInformation("Get Questions from Cache");

                questions = cachedQuestions;
            }*/
            #endregion


            var key = $"{_cachPrefix}-{pollId}";

            var questions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
                key,
                async cacheEntry => await _context.Questions
                  .Where(q => q.PollId == pollId && q.IsActive)
                  .Include(q => q.Answers)
                  .Select(q => new QuestionResponse
                  (
                      q.Id,
                      q.Content,
                      q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.Id, a.Content))
                  )).AsNoTracking()
                    .ToListAsync(cancellationToken)
                );


            return Result.Success(questions);
        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken = default)
        {
            var questions = await _context.Questions
            .Where(question => question.PollId == pollId && question.Id == questionId)
            .Include(question => question.Answers)
            .ProjectToType<QuestionResponse>()
            .AsNoTracking()
            .SingleOrDefaultAsync(cancellationToken);

            if (questions is null)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            }

            return Result.Success(questions);
        }

        public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            }
            var questionIsExists = await _context.Questions.AnyAsync(q => q.Content == request.Content && q.PollId == pollId, cancellationToken: cancellationToken);
            if (questionIsExists)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);
            }

            var question = request.Adapt<Question>();
            question.PollId = pollId;

            //request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

            await _context.AddAsync(question, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            //await _cacheService.RemoveAsync($"{_cachPrefix}-{pollId}", cancellationToken);
            await _hybridCache.RemoveAsync($"{_cachPrefix}-{pollId}", cancellationToken);

            return Result.Success(question.Adapt<QuestionResponse>());

        }


        public async Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellation = default)
        {
            var questionIsExist = await _context.Questions.AnyAsync(
              question => question.PollId == pollId
              && question.Id != questionId
              && question.Content == request.Content
              , cancellation);

            if (questionIsExist)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);
            }

            var question = await _context.Questions.Include(question => question.Answers)
                          .SingleOrDefaultAsync(qustion => qustion.PollId == pollId && qustion.Id == questionId);
            if (question is null)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
            }

            question.Content = request.Content;

            // Current answers that there is in DB
            var currentAnswers = question.Answers.Select(answer => answer.Content).ToList();

            // Add new answers that there is in request and not there is in DB

            // this way will return answers that there is in request and no exist in DB
            var newAnswers = request.Answers.Except(currentAnswers).ToList();
            newAnswers.ForEach(answer =>
            {
                question.Answers.Add(new Answer { Content = answer });
            });

            // currnt Answers in DB
            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });

            await _context.SaveChangesAsync(cancellation);

            //await _cacheService.RemoveAsync($"{_cachPrefix}-{pollId}", cancellation);
            await _hybridCache.RemoveAsync($"{_cachPrefix}-{pollId}", cancellation);

            return Result.Success();

        }


        public async Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellation = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellation);
            if (!pollIsExists)
            {
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            }

            var question = await _context.Questions.SingleOrDefaultAsync(x => x.PollId == pollId && x.Id == questionId, cancellation);
            if (question is null)
                return Result.Failure(QuestionErrors.QuestionNotFound);

            question.IsActive = !question.IsActive;

            await _context.SaveChangesAsync(cancellation);

            //await _cacheService.RemoveAsync($"{_cachPrefix}-{pollId}", cancellation);
            await _hybridCache.RemoveAsync($"{_cachPrefix}-{pollId}", cancellation);


            return Result.Success();
        }


    }
}
