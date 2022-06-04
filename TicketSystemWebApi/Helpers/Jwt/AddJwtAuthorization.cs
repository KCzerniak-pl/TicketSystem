using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TicketSystemWebApi.Helpers.Jwt
{
    public static class JwtAuthorizationExtension
    {
        public static void AddJwtAuthorization(IServiceCollection services)
        {
            // Get data from "JwtConfig".
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            JwtConfig jwtConfig = serviceProvider.GetService<IOptions<JwtConfig>>()!.Value;

            // Remove default claims.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Validate token.
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Sets the default scheme to use when authenticating.   
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Sets the default scheme to use when challenging.   
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Sets the default fallback scheme.

                })
                .AddJwtBearer(jwt =>
                {
                    // Secret string used to sign and verify JWT tokens.
                    var key = Encoding.ASCII.GetBytes(jwtConfig!.Secret);

                    // Token should be stored after a successful authorization.
                    jwt.SaveToken = true;

                    // Set of parameters that are used by validating a token.
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // Remove delay of token when expire.
                    };
                });
        }
    }
}
