using Microsoft.AspNetCore.Identity.Data;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellation)
        {
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellation);
            return authResult is null ? BadRequest("Invalid email or pass") : Ok(authResult);
        }
    }
}
