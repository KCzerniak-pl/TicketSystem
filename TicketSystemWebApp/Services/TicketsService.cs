using TicketSystemWebApp.Mapping;
using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public class TicketsService : ITicketsService
    {
        // URL for WebApi (address and port can be verified in launchSettings.json in WebAPI).
        private readonly string _url = "http://host.docker.internal:5173";

        // Retrieving data about all tickets or all tickets for selected user.
        public async Task<TicketViewModel[]> GetTicketsAsync(int skip, int take, Guid userID = default)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about all tickets or all tickets for selected user (used service from TicketSystemWebApiClient).
                IEnumerable<GetTicketsDto> tickets = await apiClient.GetTicketsAsync(skip, take, userID);

                // Mapping DTO to object used by the application - tickets.
                return tickets.Select(dto => TicketMapping.GetTicketsFromDto(dto)).ToArray();
            }
        }

        // Retrieving data about count of tickets.
        public async Task<int> GetTicketsCountAsync(Guid userID = default!)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about count of tickets (used service from TicketSystemWebApiClient).
                return await apiClient.GetTicketsCountAsync(userID);
            }
        }

        // Retrieving data about selected ticket.
        public async Task<TicketViewModel> GetTicketAsync(Guid ticketID, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about selected ticket (used service from TicketSystemWebApiClient).
                GetTicketDto ticket = await apiClient.GetTicketAsync(ticketID, userID);

                // Mapping DTO to object used by the application - selected ticket.
                return TicketMapping.GetTicketFromDto(ticket);
            }
        }

        // Retrieving data about all categories.
        public async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Retrieving data about all categories (used service from TicketSystemWebApiClient)..
                IEnumerable<GetCategoriesDto> categories = await apiClient.GetCategoriesAsync();

                // Mapping DTO to object used by the application - categories.
                return categories.Select(dto => CategoryMapping.GetCategoriesFromDto(dto)).ToList();
            }
        }

        // Add new ticket.
        public async Task PostTicketAsync(TicketNewViewModel ticket, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and add new ticket (used service from TicketSystemWebApiClient).
                await apiClient.PostTicketAsync(TicketMapping.PostTicketToDto(ticket, userID));
            }
        }

        // Add new message for selected ticket.
        public async Task PostMessageAsync(MessageNewViewModel message, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Przemapowanie z obiektu używanego przez klienta na DTO i przesłanie informacji o nowym zdjęciu.

                // Mapping data to DTO and add new message for selected ticket (used service from TicketSystemWebApiClient).
                await apiClient.PostMessageAsync(MessageMapping.PostMessageToDto(message, userID));
            }
        }

        // Update status for ticket.
        public async Task PutTicketStatusAsync(TicketStatusUpdateViewModel ticket, Guid userID, Guid technicianID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and update status for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketStatusAsync(TicketMapping.PutTicketStatusToDto(ticket, userID, technicianID));
            }
        }

        // Update title for ticket.
        public async Task PutTicketTitleAsync(TicketTitleUpdateViewModel ticket, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and update title for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketTitleAsync(TicketMapping.PutTicketTitleToDto(ticket, userID));
            }
        }

        // Update category for ticket.
        public async Task PutTicketCategoryAsync(TicketCategoryUpdateViewModel ticket, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and update category for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketCategoryAsync(TicketMapping.PutTicketCategoryToDto(ticket, userID));
            }
        }

        // Update technician for ticket.
        public async Task PutTicketTechnicianAsync(TicketTechnicianUpdateViewModel ticket, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and update category for ticket (used service from TicketSystemWebApiClient).
                await apiClient.PutTicketTechnicianAsync(TicketMapping.PutTicketTechnicianToDto(ticket, userID));
            }
        }

        // Delete ticket.
        public async Task DeleteTicketAsync(Guid ticketID, Guid userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // API client.
                TicketSystemWebApiClient apiClient = new TicketSystemWebApiClient(_url, httpClient);

                // Mapping data to DTO and delete ticket (used service from TicketSystemWebApiClient).
                await apiClient.DeleteTicketAsync(TicketMapping.DeleteTicketToDto(ticketID, userID));
            }
        }
    }
}
