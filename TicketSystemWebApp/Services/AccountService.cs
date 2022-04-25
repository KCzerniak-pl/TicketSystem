using TicketSystemWebApp.Mapping;
using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public class AccountService : IAccountService
    {
        // URL for WebApi (address and port can be verified in launchSettings.json in WebAPI).
        private readonly string _url = "http://localhost:5173";

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
        public async Task<UserViewModel> GetUserDataAsync(Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about selected user (used service from TicketSystemWebApiClient).
                GetUsersDto user = await apiClient.GetUserDataAsync(userID);

                // Mapping DTO to object used by the application - user.
                return AccountMapping.GetUserFromDto(user);
            }
        }
    }
}
