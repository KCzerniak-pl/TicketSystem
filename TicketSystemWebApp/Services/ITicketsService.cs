using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public interface ITicketsService
    {
        Task<TicketViewModel[]> GetTicketsAsync(string jwt, int skip, int take, bool showAll);
        Task<int> GetTicketsCountAsync(string jwt, bool showAll);
        Task<TicketViewModel> GetTicketAsync(string jwt, Guid ticketID);
        Task<List<CategoryViewModel>> GetCategoriesAsync(string jwt);
        Task PostTicketAsync(string jwt, TicketNewViewModel ticket);
        Task PostMessageAsync(string jwt, MessageNewViewModel message); 
        Task PutTicketStatusAsync(string jwt, TicketStatusUpdateViewModel ticket, Guid technicianID);
        Task PutTicketTitleAsync(string jwt, TicketTitleUpdateViewModel ticket);
        Task PutTicketCategoryAsync(string jwt, TicketCategoryUpdateViewModel ticket);
        Task PutTicketTechnicianAsync(string jwt, TicketTechnicianUpdateViewModel ticket);
        Task DeleteTicketAsync(string jwt, Guid ticketID);
    }
}
