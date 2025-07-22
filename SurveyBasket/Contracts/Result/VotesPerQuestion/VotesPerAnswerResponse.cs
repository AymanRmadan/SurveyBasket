namespace SurveyBasket.Contracts.Result.VotesPerQuestion
{
    public record VotesPerAnswerResponse(
        string Answer,
        int Count
        );
}
