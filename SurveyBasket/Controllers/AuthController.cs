using SurveyBasket.Contracts.Authentications.Auth.Requests;
using SurveyBasket.Contracts.Logins.Request;


namespace SurveyBasket.Controllers
{
    [Route("[controller]")]

    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellation)
        {
            _logger.LogInformation("Logging with email : {email} and password : {password}", request.email, request.password);
            var authResult = await _authService.GetTokenAsync(request.email, request.password, cancellation);
            //return authResult.IsSuccess ? Ok(authResult.Value) : BadRequest(authResult.Error);
            return authResult.IsSuccess
                ? Ok(authResult.Value)
                : authResult.ToProblem();
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellation);
            //return authResult is null ? BadRequest("Invalid token") : Ok(authResult);
            return authResult.IsSuccess
                ? Ok(authResult.Value)
                : authResult.ToProblem();
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
        {
            var revokedResult = await _authService.RevokeRefreshTokenAsync(request.token, request.refreshToken, cancellation);
            return revokedResult.IsSuccess
                ? Ok()
                : revokedResult.ToProblem();


        }
    }
}
