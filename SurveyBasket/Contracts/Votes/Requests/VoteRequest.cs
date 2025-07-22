using SurveyBasket.Contracts.Votes.Requests;

namespace SurveyBasket.Contracts.Requests
{
    public record VoteRequest(IEnumerable<VoteAnswerRequest> Answers);

}
