namespace SurveyBasket.Contracts.Result.VotesPerQuestion
{
    public record VotesPerQuestionResponse(
        string Question,
        IEnumerable<VotesPerAnswerResponse> SelectedAnswers
        );
}
