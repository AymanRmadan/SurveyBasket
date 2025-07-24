using SurveyBasket.Contracts.Authentications.Auth.Requests;
using SurveyBasket.Contracts.Authentications.Emails;
using SurveyBasket.Contracts.Authentications.Register;
using SurveyBasket.Contracts.Logins.Request;


namespace SurveyBasket.Controllers
{
    [Route("[controller]")]

    [ApiController]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellation)
        {
            var result = await _authService.RegisterAsync(request, cancellation);
            return result.IsSuccess
                ? Ok()
                : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellation)
        {
            _logger.LogInformation("Logging with email : {email} and password : {password}", request.email, request.password);
            var authResult = await _authService.GetTokenAsync(request.email, request.password, cancellation);
            //return authResult.IsSuccess ? Ok(authResult.Value) : BadRequest(authResult.Error);
            return authResult.IsSuccess
                ? Ok(authResult.Value)
                : authResult.ToProblem();
        }


        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await _authService.ConfirmEmailAsync(request);
            return result.IsSuccess
                ? Ok()
                : result.ToProblem();
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellation);
            //return authResult is null ? BadRequest("Invalid token") : Ok(authResult);
            return authResult.IsSuccess
                ? Ok(authResult.Value)
                : authResult.ToProblem();
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
        {
            var revokedResult = await _authService.RevokeRefreshTokenAsync(request.token, request.refreshToken, cancellation);
            return revokedResult.IsSuccess
                ? Ok()
                : revokedResult.ToProblem();
        }


    }
}
