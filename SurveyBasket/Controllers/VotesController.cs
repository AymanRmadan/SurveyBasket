using SurveyBasket.Contracts.Requests;
using SurveyBasket.Services.Questions;
using SurveyBasket.Services.Votes;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    //  [Authorize]
    public class VotesController(IQuestionService questionService, IVoteServics voteServics) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;
        private readonly IVoteServics _voteServics = voteServics;

        [HttpGet("")]
        //[ResponseCache(Duration = 60)]
        // [OutputCache(Duration = 60)]
        public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            // var user = User.GetUserId();
            var user = "81cd0080-4b11-40d0-8a28-ec840663216e";
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
