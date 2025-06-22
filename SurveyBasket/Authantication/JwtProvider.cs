
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Authantication
{
    public class JwtProvider : IJwtProvider
    {
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
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TxNj0JuYaIjWbkJDZ27QAqNCLVeACRjV"));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            // expiration for key
            var expireIn = 30;
            var expirationDate = DateTime.UtcNow.AddMinutes(expireIn);

            var token = new JwtSecurityToken(
                issuer: "SurveyBasketApp",
                audience: "SurveyBasketApp users",
                claims: claims,
                expires: expirationDate,
                signingCredentials: signingCredentials
                );

            return (token: new JwtSecurityTokenHandler().WriteToken(token), expireIn: expireIn);
        }

    }
}
