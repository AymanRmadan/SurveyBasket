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

            //return Ok(polls.MapToPollResponse());
            return Ok(polls);
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

            //if there is prop in response is not the same name in model use this way
            var config = new TypeAdapterConfig();
            config.NewConfig<Poll, PollResponse>()
                .Map(dest => dest.Notes, src => src.Description);
            // Mapster Mapping
            var response = poll.Adapt<PollResponse>(config);
            return Ok(response);
        }

        [HttpPost("")]
        public IActionResult Add(CreatePollRequest request)
        {
            /*// var newPoll = _pollService.Add(request.MapToPoll());
             var newPoll = _pollService.Add(request);
             // return Ok(newPoll);

             return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);*/

            return Ok(request);

        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CreatePollRequest request)
        {
            /*var isUpdate = _pollService.Update(id, request.MapToPoll());

            if (!isUpdate)
                return NotFound();*/

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
