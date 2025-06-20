using Mapster;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;

namespace SurveyBasket.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var polls = await pollService.GetAllAsync();
            var response = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var poll = await _pollService.GetAsync(id);
            // return poll is null ? NotFound() : Ok(poll.MapToPollResponse());
            if (poll is null)
            {
                return NotFound();
            }
            // Mapster Mapping
            var response = poll.Adapt<PollResponse>();
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody] CreatePollRequest request)
        //[FromServices] IValidator<CreatePollRequest> validator)
        {

            //var validationResult = validator.Validate(request);
            //if (!validationResult.IsValid)
            //{
            //    var modelState = new ModelStateDictionary();
            //    validationResult.Errors.ForEach(x => modelState.AddModelError(x.PropertyName, x.ErrorMessage));
            //    return ValidationProblem(modelState);
            //}

            // var newPoll = _pollService.Add(request.MapToPoll());
            var newPoll = await _pollService.AddAsync(request.Adapt<Poll>());
            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CreatePollRequest request)
        {
            // Manual Map
            //var isUpdate = _pollService.Update(id, request.MapToPoll());

            // Mapster Map
            var isUpdate = _pollService.Update(id, request.Adapt<Poll>());

            if (!isUpdate)
                return NotFound();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _pollService.Delete(id);
            if (!isDeleted)
                return NotFound();
            return NoContent();
        }



    }
}
