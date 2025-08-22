using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Abstractions.Consts;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Services.Polls;

namespace SurveyBasket.Controllers
{

    [ApiVersion(1, Deprecated = true)]
    [ApiVersion(2)]
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet("")]
        [HasPermission(Permissions.GetPolls)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await pollService.GetAllAsync(cancellationToken));
        }


        [MapToApiVersion(1)]
        [HttpGet("current")]
        [Authorize(Roles = DefaultRoles.Member)]
        [EnableRateLimiting(RateLimiters.UserLimiter)]
        public async Task<IActionResult> GetCurrentV1(CancellationToken cancellationToken)
        {
            return Ok(await _pollService.GetCurrentAsyncV1(cancellationToken));
        }

        [MapToApiVersion(2)]
        [HttpGet("current")]
        [Authorize()]
        public async Task<IActionResult> GetCurrentV2(CancellationToken cancellationToken)
        {
            return Ok(await _pollService.GetCurrentAsyncV2(cancellationToken));
        }


        [HttpGet("{id}")]
        [HasPermission(Permissions.GetPolls)]

        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _pollService.GetAsync(id);
            //// return poll is null ? NotFound() : Ok(poll.MapToPollResponse());
            //if (poll is null)
            //{
            //    return NotFound();
            //}
            //// Mapster Mapping
            //var response = poll.Adapt<PollResponse>();

            //return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();

        }

        [HttpPost("")]
        [HasPermission(Permissions.AddPolls)]

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

            var result = await _pollService.AddAsync(request, cancellation);
            return result.IsSuccess
                ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();

        }

        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> Update(int id, PollRequest request, CancellationToken cancellation)
        {
            // Manual Map
            //var isUpdate = _pollService.Update(id, request.MapToPoll());

            // Mapster Map
            var result = await _pollService.UpdateAsync(id, request, cancellation);


            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }

        [HttpDelete("{id}")]
        [HasPermission(Permissions.DeletePolls)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)

        {
            var result = await _pollService.DeleteAsync(id, cancellation);
            //if (!!result)
            //    return NotFound();
            //return NoContent();

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }


        [HttpPut("{id}/toggle-publish")]
        [HasPermission(Permissions.UpdatePolls)]
        public async Task<IActionResult> TogglePublish([FromRoute] int id, CancellationToken cancellation)

        {
            var result = await _pollService.TogglePublishStatusAsync(id, cancellation);

            return result.IsSuccess
                ? NoContent()
                : result.ToProblem();
        }



    }
}
