using SurveyBasket.Contracts.Questions;
using SurveyBasket.Services.Question;

namespace SurveyBasket.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("{questionId}")]
        public async Task<IActionResult> Get([FromRoute] int pollId, int questionId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAsync(pollId, questionId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAsync([FromRoute] int pollId, QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(pollId, request, cancellationToken);

            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { pollId = pollId, id = result.Value.Id }, result.Value)
                : result.ToProblem();

            //return result.Error.Equals(QuestionErrors.DuplicatedQuestionContent)
            //    ? result.ToProblem(StatusCodes.Status404NotFound)
            //    : result.ToProblem(StatusCodes.Status404NotFound);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int pollId, [FromRoute] int id, QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }


        [HttpPut("{questionId}/toggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, int questionId, CancellationToken cancellation)

        {
            var result = await _questionService.ToggleStatusAsync(pollId, questionId, cancellation);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

    }
}
