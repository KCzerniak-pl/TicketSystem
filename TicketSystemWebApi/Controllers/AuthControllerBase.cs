using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketSystemWebApi.Helpers.Jwt;

namespace TicketSystemWebApi.Controllers
{
    public abstract class AuthControllerBase : ControllerBase
    {
        protected readonly JwtConfig _jwtConfig;

        protected AuthControllerBase(IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        // JWT - create token.
        protected string GenerateJwtToken(Database.Entities.User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Secret string used to sign and verify JWT tokens.
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            // Information which used to create a security token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserID", user.UserID.ToString()),
                    new Claim("UserName", string.Format("{0} {1}", user.FirstName, user.LastName)),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                // Generate token that is valid for the set time.
                Expires = DateTime.UtcNow.AddHours(6),

                // Cryptographic key and security algorithms that are used to generate a digital signature.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create token.
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
