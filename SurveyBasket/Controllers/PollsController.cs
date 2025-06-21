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
        public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellation)
        //[FromServices] IValidator<CreatePollRequest> validator)
        {

            #region Comments
            //var validationResult = validator.Validate(request);
            //if (!validationResult.IsValid)
            //{
            //    var modelState = new ModelStateDictionary();
            //    validationResult.Errors.ForEach(x => modelState.AddModelError(x.PropertyName, x.ErrorMessage));
            //    return ValidationProblem(modelState);
            //}

            // var newPoll = _pollService.Add(request.MapToPoll()); 
            #endregion

            var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellation);
            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PollRequest request, CancellationToken cancellation)
        {
            // Manual Map
            //var isUpdate = _pollService.Update(id, request.MapToPoll());

            // Mapster Map
            var isUpdate = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellation);

            if (!isUpdate)
                return NotFound();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)

        {
            var isDeleted = await _pollService.DeleteAsync(id, cancellation);
            if (!isDeleted)
                return NotFound();
            return NoContent();
        }


        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellation)

        {
            var toggle = await _pollService.TogglePublishStatusAsync(id, cancellation);
            if (!toggle)
                return NotFound();
            return NoContent();
        }



    }
}
