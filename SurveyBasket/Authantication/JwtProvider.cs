
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authantication
{
    public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
            // this things will return in response with token 
            Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub , user.Id),
                new(JwtRegisteredClaimNames.Email , user.Email!),
                new(JwtRegisteredClaimNames.GivenName , user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName , user.LastName),
                new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                ];

            //Generate Key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // expiration for key
            var expireIn = _jwtOptions.ExpiryMinutes;
            var expirationDate = DateTime.UtcNow.AddMinutes(expireIn);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expirationDate,
                signingCredentials: signingCredentials
                );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expireIn: expireIn * 60);
        }

    }
}
