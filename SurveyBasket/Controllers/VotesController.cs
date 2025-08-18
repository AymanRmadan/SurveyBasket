using SurveyBasket.Abstractions.Consts;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Services.Questions;
using SurveyBasket.Services.Votes;

namespace SurveyBasket.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize(Roles = DefaultRoles.Member)]
public class VotesController(IQuestionService questionService, IVoteServices voteService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteServices _voteService = voteService;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _questionService.GetAvailableAsync(pollId, userId!, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var result = await _voteService.AddVoteAsync(pollId, User.GetUserId()!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
}