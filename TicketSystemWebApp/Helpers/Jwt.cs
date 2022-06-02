using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;

namespace TicketSystemWebApp.Helpers
{
    public class Jwt
    {
        // Get data from JWT.
        public static T GetObjectFromJwt<T>(string jwt, string key)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = jwtHandler.ReadJwtToken(jwt);

            string? value = token.Claims.FirstOrDefault(x => x.Type == key)?.Value;

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return value == null ? default(T)! : (T)converter.ConvertFrom(value!)!;
        }

        // Add JWT to HTTP header.
        public  static void AddJwtToHeader(HttpClient httpClient, string jwt)
        {
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        }
    }
}
