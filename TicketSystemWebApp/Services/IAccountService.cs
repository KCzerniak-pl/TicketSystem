using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public interface IAccountService
    {
        Task<LoginResponseDto> LoginAsync(LoginViewModel user);
        Task<UserViewModel> GetUserDataAsync(Guid userID);
    }
}
