﻿using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class AccountMapping
    {
        // Mapping DTO to object used by the application - user.
        internal static UserViewModel GetUserFromDto(GetUsersDto dto)
        {
            UserViewModel returnValue = new UserViewModel();

            returnValue.UserID = dto.UserID;
            returnValue.FirstName = dto.FirstName;
            returnValue.LastName = dto.LastName;
            returnValue.Email = dto.Email;
            returnValue.DateTimeCreated = dto.DateTimeCreated.ToString("dd/MM/yyyy HH:mm:ss");
            returnValue.Role = new RoleViewModel()
            {
                RoleID = dto.Role.RoleID,
                RoleName = dto.Role.RoleName,
                ShowAll = dto.Role.ShowAll,
                CanAccepted = dto.Role.CanAccepted
            };

            return returnValue;
        }

        // Mapping DTO to object used by the application - user role.
        internal static RoleViewModel GetRoleFromDto(GetRoleDto message)
        {
            RoleViewModel returnValue = new RoleViewModel();

            returnValue.RoleID = message.RoleID;
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
