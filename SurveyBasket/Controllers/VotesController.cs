using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class VotesController(IQuestionService questionService, IVoteServics voteServics) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteServics _voteServics = voteServics;

        [HttpGet("")]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var user = User.GetUserId();
            var result = await _questionService.GetAvailableAsync(pollId, user!, cancellationToken);

            return result.IsSuccess
                  ? Ok(result)
                  : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
        {
            var user = User.GetUserId();
            var result = await _voteServics.AddVoteAsync(pollId, user!, request, cancellationToken);

            return result.IsSuccess
                  ? Created()
                  : result.ToProblem();
        }

    }
}
