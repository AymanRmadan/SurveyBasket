namespace SurveyBasket.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly List<Poll> _polls =
            [
            new Poll
            {
                Id = 1,
                Title="Poll 1",
                Description="My first Poll"
            },
              new Poll
            {
                Id = 2,
                Title="Poll 2",
                Description="My second Poll"
            }

            ];

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_polls);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var poll = _polls.SingleOrDefault(pol => pol.Id == id);
            return poll is null ? NotFound() : Ok(poll);
        }

    }
}
