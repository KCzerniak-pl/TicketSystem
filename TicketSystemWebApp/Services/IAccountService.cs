using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Services
{
    public interface IAccountService
    {
        Task<LoginResponseDto> LoginAsync(LoginViewModel user);
        Task<UserViewModel> GetUserDataAsync(string jwt);
        Task<List<UserViewModel>> GetTechniciansAsync(string jwt);
    }
}
