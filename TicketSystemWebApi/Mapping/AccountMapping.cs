using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Mapping
{
    public class AccountMapping
    {
        // Mapping data from database to DTO - user list.
        internal static GetUsersDto GetUsersToDto(Database.Entities.User user)
        {
            GetUsersDto returnValue = new GetUsersDto();

            returnValue.UserId = user.UserId;
            returnValue.FirstName = user.FirstName!;
            returnValue.LastName = user.LastName!;
            returnValue.Email = user.Email!;
            returnValue.DateTimeCreated = user.DateTimeCreated;
            returnValue.Role = new GetRoleDto()
            {
                RoleId = user.Role!.RoleId,
                RoleName = user.Role.RoleName!,
                ShowAll = user.Role.ShowAll,
                CanAccepted = user.Role.CanAccepted,
                Technician = user.Role.Technician
            };

            return returnValue;
        }

        // Mapping data from database to DTO - user role.
        internal static GetRoleDto GetRoleToDto(Database.Entities.UserRole role)
        {
            GetRoleDto returnValue = new GetRoleDto();

            returnValue.RoleId = role.RoleId;
            returnValue.RoleName = role.RoleName!;
            returnValue.CanAccepted = role.CanAccepted;
            returnValue.Technician = role.Technician;

            return returnValue;
        }

        // Mapping data from database to DTO - login.
        internal static LoginResponseDto LoginResponseToDto(bool success, string error, string jwt = default!)
        {
            LoginResponseDto returnValue = new LoginResponseDto();

            if (success)
            {
                returnValue.Jwt = jwt;
            }
            returnValue.Success = success;
            returnValue.Error = error;

            return returnValue;
        }
    }
}
