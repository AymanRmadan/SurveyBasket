using SurveyBasket.Contracts.Answers.Responses;

namespace SurveyBasket.Contracts.Questions.Responses
{
    public record QuestionResponse(
        int Id,
        string Content,
        IEnumerable<AnswerResponse> Answers
        );

}
