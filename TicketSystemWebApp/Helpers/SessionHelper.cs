using Newtonsoft.Json;

namespace TicketSystemWebApp.Helpers
{
    public class SessionHelper
    {
        // Save data in session / cookies.
        public static void SetObjectAsJson(HttpContext httpContext, string key, string value, bool cookies = false)
        {
            value = JsonConvert.SerializeObject(value);

            // Save data in session.
            httpContext.Session.SetString(key, value);

            if (cookies)
            {
                // Save data in cookies.
                DateTimeOffset dataTimeExpires = DateTimeOffset.Now.AddHours(24);
                httpContext.Response.Cookies.Append(key, value, new CookieOptions { Expires = dataTimeExpires });
            }
        }

        static readonly JsonSerializer _serializer = new JsonSerializer();

        // Get data from session / cookies.
        public static T? GetObjectFromJson<T>(HttpContext httpContext, string key)
        {
            // Get data from session.
            string? value = httpContext.Session.GetString(key);

            if (value == null)
            {
                // Get data from cookies.
                value = httpContext.Request.Cookies[key];
            }

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        // Delete session / cookies.
        public static void Logout(HttpContext httpContext)
        {
            // Delete session.
            httpContext.Session.Clear();

            // Delete cookies.
            foreach (string cookieKey in httpContext.Request.Cookies.Keys)
            {
                httpContext.Response.Cookies.Delete(cookieKey);
            }
        }

        // Get data about authorization from session / cookies.
        public static bool CheckAuthorization(HttpContext httpContext)
        {
            return SessionHelper.GetObjectFromJson<bool>(httpContext, "Authorization");
        }
    }
}
