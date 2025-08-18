using SurveyBasket.Contracts.Users.Requests;

namespace SurveyBasket.Controllers;
// [Route("api/[controller]")]
[Route("me")]
[ApiController]
[Authorize]
public class AcountsController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;


    [HttpGet("")]
    //this mean no Authorize
    [AllowAnonymous]
    public async Task<IActionResult> Profile()
    {
        var result = await _userService.GetProfileAsync(User.GetUserId()!);
        return Ok(result.Value);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> Profile([FromBody] UpdateProfileRequest request)
    {
        await _userService.UpdateProfileAsync(User.GetUserId()!, request);
        return NoContent();
    }

    [HttpPut("change-pass")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
