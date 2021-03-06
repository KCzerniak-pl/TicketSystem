using TicketSystemWebApp.Helpers;
using TicketSystemWebApp.Mapping;
using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public class TicketsService : ITicketsService
    {
        // URL for WebApi (address and port can be verified in launchSettings.json in WebAPI).
        private readonly string _url = "http://host.docker.internal:5173";

        // Retrieving data about all tickets or all tickets for selected user.
        public async Task<TicketViewModel[]> GetTicketsAsync(string jwt, int skip, int take, bool showAll)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Retrieving data about all tickets or all tickets for selected user (used service from TicketSystemWebApiClient).
                IEnumerable<GetTicketsDto> tickets = await apiClient.GetTicketsAsync(skip, take, showAll ? null : userId);

                // Mapping DTO to object used by the application - tickets.
                return tickets.Select(dto => TicketMapping.GetTicketsFromDto(dto)).ToArray();
            }
        }

        // Retrieving data about count of tickets.
        public async Task<int> GetTicketsCountAsync(string jwt, bool showAll)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Retrieving count about all ticket or only ticket for selected user (used service from TicketSystemWebApiClient).
                return await apiClient.GetTicketsCountAsync(showAll ? null : userId);
            }
        }

        // Retrieving data about selected ticket.
        public async Task<TicketViewModel> GetTicketAsync(string jwt, Guid ticketId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Retrieving data about selected ticket (used service from TicketSystemWebApiClient).
                GetTicketDto ticket = await apiClient.GetTicketAsync(ticketId, userId);

                // Mapping DTO to object used by the application - selected ticket.
                return TicketMapping.GetTicketFromDto(ticket);
            }
        }

        // Retrieving data about all categories.
        public async Task<List<CategoryViewModel>> GetCategoriesAsync(string jwt)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about all categories (used service from TicketSystemWebApiClient)..
                IEnumerable<GetCategoriesDto> categories = await apiClient.GetCategoriesAsync();

                // Mapping DTO to object used by the application - categories.
                return categories.Select(dto => CategoryMapping.GetCategoriesFromDto(dto)).ToList();
            }
        }

        // Add new ticket.
        public async Task PostTicketAsync(string jwt, TicketNewViewModel ticket)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and add new ticket (used service from TicketSystemWebApiClient).
                await apiClient.PostTicketAsync(TicketMapping.PostTicketToDto(ticket, userId));
            }
        }

        // Add new message for selected ticket.
        public async Task PostMessageAsync(string jwt, MessageNewViewModel message)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and add new message for selected ticket (used service from TicketSystemWebApiClient).
                await apiClient.PostMessageAsync(MessageMapping.PostMessageToDto(message, userId));
            }
        }

        // Update status for ticket.
        public async Task PutTicketStatusAsync(string jwt, TicketStatusUpdateViewModel ticket, Guid technicianId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and update status for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketStatusAsync(TicketMapping.PutTicketStatusToDto(ticket, userId, technicianId));
            }
        }

        // Update title for ticket.
        public async Task PutTicketTitleAsync(string jwt, TicketTitleUpdateViewModel ticket)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and update title for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketTitleAsync(TicketMapping.PutTicketTitleToDto(ticket, userId));
            }
        }

        // Update category for ticket.
        public async Task PutTicketCategoryAsync(string jwt, TicketCategoryUpdateViewModel ticket)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and update category for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketCategoryAsync(TicketMapping.PutTicketCategoryToDto(ticket, userId));
            }
        }

        // Update technician for ticket.
        public async Task PutTicketTechnicianAsync(string jwt, TicketTechnicianUpdateViewModel ticket)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and update category for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketTechnicianAsync(TicketMapping.PutTicketTechnicianToDto(ticket, userId));
            }
        }

        // Delete ticket.
        public async Task DeleteTicketAsync(string jwt, Guid ticketId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Add JWT to HTTP header.
                Jwt.AddJwtToHeader(httpClient, jwt);

                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Get userId from JWT.
                Guid userId = Jwt.GetObjectFromJwt<Guid>(jwt, "UserId");

                // Mapping data to DTO and delete ticket (used service from TicketSystemWebApiClient).
                await apiClient.DeleteTicketAsync(TicketMapping.DeleteTicketToDto(ticketId, userId));
            }
        }
    }
}
