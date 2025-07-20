using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Contracts.Questions.Responses;
using SurveyBasket.Errors;
using SurveyBasket.Persistence;

namespace SurveyBasket.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly AppDbContext _context;

        public QuestionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _context.Polls.AnyAsync(p => p.Id == pollId, cancellationToken: cancellationToken);
            if (!pollIsExists)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            }
            /*    // this will return all columns from DB
                var questions = await _context.Questions
                    .Where(question => question.PollId == pollId)
                    .Include(question => question.Answers)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);*/


            // this will return only columns that i will selet it from DB
            // this wat for big data
            var questions = await _context.Questions
                .Where(question => question.PollId == pollId)
                .Include(question => question.Answers)
               /* .Select(question => new QuestionResponse(
                    question.Id,
                    question.Content,
                    question.Answers.Select(answer => new AnswerResponse(
                        answer.Id,
                        answer.Content
                        ))
                    ))*/
               //ANother way to use Projection istead of select
               .ProjectToType<QuestionResponse>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(questions);
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


            return Result.Success();
        }


    }
}
