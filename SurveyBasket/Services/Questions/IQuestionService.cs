using SurveyBasket.Contracts.Questions;
using SurveyBasket.Contracts.Questions.Responses;

namespace SurveyBasket.Services.Questions
{
    public interface IQuestionService
    {
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest request, CancellationToken cancellation = default);

        // this function To change IsActive from true to false and Vis versa that specific questions
        Task<Result> ToggleStatusAsync(int pollId, int questionId, CancellationToken cancellation = default);

    }
}
