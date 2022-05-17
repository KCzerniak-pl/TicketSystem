using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class TicketMapping
    {
        // Mapping DTO to object used by the application - selected ticket.
        internal static TicketViewModel GetTicketFromDto(GetTicketDto dto)
        {
            TicketViewModel returnValue = new TicketViewModel();

            returnValue.TicketID = dto.TicketID;
            returnValue.StatusID = dto.StatusID;
            returnValue.StatusName = dto.StatusName;
            returnValue.CategoryID = dto.CategoryID;
            returnValue.CategoryName = dto.CategoryName;
            returnValue.DateTimeCreated = dto.DateTimeCreated;
            returnValue.DateTimeModified = dto.DateTimeModified;
            returnValue.Title = dto.Title;
            returnValue.Messages = dto.Messages.Select(p => MessageMapping.GetMessagesFromDto(p)).ToList();
            returnValue.UserID = dto.UserID;
            returnValue.UserName = dto.UserName;
            returnValue.Email = dto.Email;
            returnValue.TechnicianID = dto.TechnicianID;

            return returnValue;
        }

        // Mapping DTO to object used by the application - tickets.
        internal static TicketViewModel GetTicketsFromDto(GetTicketsDto dto)
        {
            TicketViewModel returnValue = new TicketViewModel();

            returnValue.TicketID = dto.TicketID;
            returnValue.StatusID = dto.StatusID;
            returnValue.StatusName = dto.StatusName;
            returnValue.CategoryName = dto.CategoryName;
            returnValue.DateTimeCreated = dto.DateTimeCreated;
            returnValue.DateTimeModified = dto.DateTimeModified;
            returnValue.Title = dto.Title;
            returnValue.UserName = dto.UserName;

            return returnValue;
        }

        // Mapping data to DTO - new ticket.
        internal static PostTicketDto PostTicketToDto(TicketNewViewModel ticket, Guid userID)
        {
            PostTicketDto returnValue = new PostTicketDto();

            returnValue.UserID = userID;
            returnValue.CategoryID = ticket.CategoryID;
            returnValue.Title = ticket.Title;
            returnValue.Information = ticket.Information;

            return returnValue;
        }

        // Mapping data to DTO - update status.
        internal static PutTicketStatusDto PutTicketStatusToDto(TicketStatusUpdateViewModel ticket, Guid userID, Guid technicianID)
        {
            PutTicketStatusDto returnValue = new PutTicketStatusDto();

            returnValue.UserID = userID;
            returnValue.TicketID = ticket.TicketID;
            returnValue.StatusID = ticket.StatusID;
            returnValue.TechnicianID = technicianID;

            return returnValue;
        }

        // Mapping data to DTO - update title.
        internal static PutTicketTitleDto PutTicketTitleToDto(TicketTitleUpdateViewModel ticket, Guid userID)
        {
            PutTicketTitleDto returnValue = new PutTicketTitleDto();

            returnValue.UserID = userID;
            returnValue.TicketID = ticket.TicketID;
            returnValue.Title = ticket.Title;

            return returnValue;
        }

        // Mapping data to DTO - update category.
        internal static PutTicketCategoryDto PutTicketCategoryToDto(TicketCategoryUpdateViewModel ticket, Guid userID)
        {
            PutTicketCategoryDto returnValue = new PutTicketCategoryDto();

            returnValue.UserID = userID;
            returnValue.TicketID = ticket.TicketID;
            returnValue.CategoryID = ticket.CategoryID;

            return returnValue;
        }

        // Mapping data to DTO - update technician.
        internal static PutTicketTechnicianDto PutTicketTechnicianToDto(TicketTechnicianUpdateViewModel ticket, Guid userID)
        {
            PutTicketTechnicianDto returnValue = new PutTicketTechnicianDto();

            returnValue.UserID = userID;
            returnValue.TicketID = ticket.TicketID;
            returnValue.TechnicianID = ticket.TechnicianID;

            return returnValue;
        }

        // Mapping data to DTO - delete ticket.
        internal static DeleteTicketDto DeleteTicketToDto(Guid ticketID, Guid userID)
        {
            DeleteTicketDto returnValue = new DeleteTicketDto();

            returnValue.UserID = userID;
            returnValue.TicketID = ticketID;

            return returnValue;
        }
    }
}
