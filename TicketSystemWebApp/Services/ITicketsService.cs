using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public interface ITicketsService
    {
        Task<TicketViewModel[]> GetTicketsAsync(int skip, int take, Guid userID = default);
        Task<int> GetTicketsCountAsync(Guid userID = default);
        Task<TicketViewModel> GetTicketAsync(Guid ticketID, Guid userID);
        Task<List<CategoryViewModel>> GetCategoriesAsync();
        Task PostTicketAsync(TicketNewViewModel ticket, Guid userID);
        Task PostMessageAsync(MessageNewViewModel message, Guid userID); 
        Task PutTicketStatusAsync(TicketStatusUpdateViewModel ticket, Guid userID, Guid technicianID);
        Task PutTicketTitleAsync(TicketTitleUpdateViewModel ticket, Guid userID);
        Task PutTicketCategoryAsync(TicketCategoryUpdateViewModel ticket, Guid userID);
        Task PutTicketTechnicianAsync(TicketTechnicianUpdateViewModel ticket, Guid userID);
        Task DeleteTicketAsync(Guid ticketID, Guid userID);
    }
}
