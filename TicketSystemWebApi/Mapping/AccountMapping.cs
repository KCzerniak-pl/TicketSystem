using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Mapping
{
    public class AccountMapping
    {
        // Mapping data from database to DTO - user list.
        internal static GetUsersDto GetUsersToDto(Database.Entities.User user)
        {
            GetUsersDto returnValue = new GetUsersDto();

            returnValue.UserID = user.UserID;
            returnValue.FirstName = user.FirstName;
            returnValue.LastName = user.LastName;
            returnValue.Email = user.Email;
            returnValue.DateTimeCreated = user.DateTimeCreated;
            returnValue.Role = new GetRoleDto()
            {
                RoleID = user.Role.RoleID,
                RoleName = user.Role.RoleName,
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

            returnValue.RoleID = role.RoleID;
            returnValue.RoleName = role.RoleName;
            returnValue.CanAccepted = role.CanAccepted;
            returnValue.Technician = role.Technician;

            return returnValue;
        }

        // Mapping data from database to DTO - login.
        internal static LoginResponseDto LoginResponseToDto(bool success, string error, Database.Entities.User user = default!, string jwtToken = default!)
        {
            LoginResponseDto returnValue = new LoginResponseDto();

            if (user != default)
            {
                returnValue.Token = jwtToken;
                returnValue.UserID = user.UserID;
                returnValue.UserName = string.Format("{0} {1}", user.FirstName, user.LastName);
            }
            returnValue.Success = success;
            returnValue.Error = error;

            return returnValue;
        }
    }
}
