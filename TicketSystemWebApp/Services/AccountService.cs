using TicketSystemWebApp.Helpers;
using TicketSystemWebApp.Mapping;
using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public class AccountService : IAccountService
    {
        // URL for WebApi (address and port can be verified in launchSettings.json in WebAPI).
        private readonly string _url = "http://host.docker.internal:5173";

        // logging user.
        public async Task<LoginResponseDto> LoginAsync(LoginViewModel user)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and retrieving data for login.
                return await apiClient.LoginAsync(AccountMapping.PostLoginToDto(user));
            }
        }

        // Retrieving data about selected user.
        public async Task<UserViewModel> GetUserDataAsync(string jwt)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about selected user (used service from TicketSystemWebApiClient).
                GetUsersDto user = await apiClient.GetUserDataAsync(userId);

                // Mapping DTO to object used by the application - user.
                return AccountMapping.GetUserFromDto(user);
            }
        }

        // Retrieving data about technicians.
        public async Task<List<UserViewModel>> GetTechniciansAsync(string jwt)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about technicians (used service from TicketSystemWebApiClient).
                IEnumerable<GetUsersDto> technicians = await apiClient.GetTechniciansAsync();

                // Mapping DTO to object used by the application - technicians.
                return technicians.Select(dto => AccountMapping.GetTechnicianFromDto(dto)).ToList();
            }
        }
    }
}
