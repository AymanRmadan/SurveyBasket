namespace SurveyBasket.Contracts.Result
{
    public record PollVotesResponse(
        //poll title
        string Title,
        IEnumerable<VoteResponse> Votes
        );

}
