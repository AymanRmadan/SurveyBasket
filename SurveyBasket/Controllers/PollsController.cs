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
        public IActionResult GetAll()
        {
            var polls = _pollService.GetAll();
            var response = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var poll = _pollService.Get(id);
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
        public IActionResult Add(CreatePollRequest request)
        {
            // var newPoll = _pollService.Add(request.MapToPoll());
            var newPoll = _pollService.Add(request.Adapt<Poll>());
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
