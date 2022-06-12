using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class AccountMapping
    {
        // Mapping DTO to object used by the application - user.
        internal static UserViewModel GetUserFromDto(GetUsersDto dto)
        {
            UserViewModel returnValue = new UserViewModel();

            returnValue.UserId = dto.UserId;
            returnValue.FirstName = dto.FirstName;
            returnValue.LastName = dto.LastName;
            returnValue.Email = dto.Email;
            returnValue.DateTimeCreated = dto.DateTimeCreated.ToString("dd/MM/yyyy HH:mm:ss");
            returnValue.Role = new RoleViewModel()
            {
                RoleId = dto.Role.RoleId,
                RoleName = dto.Role.RoleName,
                ShowAll = dto.Role.ShowAll,
                CanAccepted = dto.Role.CanAccepted
            };

            return returnValue;
        }

        // Mapping DTO to object used by the application - user.
        internal static UserViewModel GetTechnicianFromDto(GetUsersDto dto)
        {
            UserViewModel returnValue = new UserViewModel();

            returnValue.UserId = dto.UserId;
            returnValue.FirstName = dto.FirstName;
            returnValue.LastName = dto.LastName;

            return returnValue;
        }

        // Mapping DTO to object used by the application - user role.
        internal static RoleViewModel GetRoleFromDto(GetRoleDto message)
        {
            RoleViewModel returnValue = new RoleViewModel();

            returnValue.RoleId = message.RoleId;
            returnValue.RoleName = message.RoleName;
            returnValue.CanAccepted = message.CanAccepted;

            return returnValue;
        }

        // Mapping data to DTO - login.
        internal static LoginRequestDto PostLoginToDto(LoginViewModel user)
        {
            LoginRequestDto returnValue = new LoginRequestDto();

            returnValue.Email = user.Email;
            returnValue.Password = user.Password;

            return returnValue;
        }
    }
}
