﻿using SurveyBasket.Services.Results;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ResultsController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;

        [HttpGet("row-data")]
        public async Task<IActionResult> GetPollVotes(int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("votes-per-day")]
        public async Task<IActionResult> VotesPerDay(int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVotesPerDayAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("votes-per-q")]
        public async Task<IActionResult> VotesPerQuestion(int pollId, CancellationToken cancellationToken)
        {
            var result = await _resultService.GetVotesPerQuestionAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
