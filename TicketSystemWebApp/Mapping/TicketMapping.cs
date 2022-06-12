using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class TicketMapping
    {
        // Mapping DTO to object used by the application - selected ticket.
        internal static TicketViewModel GetTicketFromDto(GetTicketDto dto)
        {
            TicketViewModel returnValue = new TicketViewModel();

            returnValue.TicketId = dto.TicketId;
            returnValue.StatusId = dto.StatusId;
            returnValue.StatusName = dto.StatusName;
            returnValue.CategoryId = dto.CategoryId;
            returnValue.CategoryName = dto.CategoryName;
            returnValue.DateTimeCreated = dto.DateTimeCreated;
            returnValue.DateTimeModified = dto.DateTimeModified;
            returnValue.Title = dto.Title;
            returnValue.Messages = dto.Messages.Select(p => MessageMapping.GetMessagesFromDto(p)).ToList();
            returnValue.UserId = dto.UserId;
            returnValue.UserName = dto.UserName;
            returnValue.Email = dto.Email;
            returnValue.TechnicianId = dto.TechnicianId;

            return returnValue;
        }

        // Mapping DTO to object used by the application - tickets.
        internal static TicketViewModel GetTicketsFromDto(GetTicketsDto dto)
        {
            TicketViewModel returnValue = new TicketViewModel();

            returnValue.TicketId = dto.TicketId;
            returnValue.No = dto.No;
            returnValue.StatusId = dto.StatusId;
            returnValue.StatusName = dto.StatusName;
            returnValue.CategoryName = dto.CategoryName;
            returnValue.DateTimeCreated = dto.DateTimeCreated;
            returnValue.DateTimeModified = dto.DateTimeModified;
            returnValue.Title = dto.Title;
            returnValue.UserName = dto.UserName;

            return returnValue;
        }

        // Mapping data to DTO - new ticket.
        internal static PostTicketDto PostTicketToDto(TicketNewViewModel ticket, Guid userId)
        {
            PostTicketDto returnValue = new PostTicketDto();

            returnValue.UserId = userId;
            returnValue.CategoryId = ticket.CategoryId;
            returnValue.Title = ticket.Title;
            returnValue.Information = ticket.Information;

            return returnValue;
        }

        // Mapping data to DTO - update status.
        internal static PutTicketStatusDto PutTicketStatusToDto(TicketStatusUpdateViewModel ticket, Guid userId, Guid technicianId)
        {
            PutTicketStatusDto returnValue = new PutTicketStatusDto();

            returnValue.UserId = userId;
            returnValue.TicketId = ticket.TicketId;
            returnValue.StatusId = ticket.StatusId;
            returnValue.TechnicianId = technicianId;

            return returnValue;
        }

        // Mapping data to DTO - update title.
        internal static PutTicketTitleDto PutTicketTitleToDto(TicketTitleUpdateViewModel ticket, Guid userId)
        {
            PutTicketTitleDto returnValue = new PutTicketTitleDto();

            returnValue.UserId = userId;
            returnValue.TicketId = ticket.TicketId;
            returnValue.Title = ticket.Title;

            return returnValue;
        }

        // Mapping data to DTO - update category.
        internal static PutTicketCategoryDto PutTicketCategoryToDto(TicketCategoryUpdateViewModel ticket, Guid userId)
        {
            PutTicketCategoryDto returnValue = new PutTicketCategoryDto();

            returnValue.UserId = userId;
            returnValue.TicketId = ticket.TicketId;
            returnValue.CategoryId = ticket.CategoryId;

            return returnValue;
        }

        // Mapping data to DTO - update technician.
        internal static PutTicketTechnicianDto PutTicketTechnicianToDto(TicketTechnicianUpdateViewModel ticket, Guid userId)
        {
            PutTicketTechnicianDto returnValue = new PutTicketTechnicianDto();

            returnValue.UserId = userId;
            returnValue.TicketId = ticket.TicketId;
            returnValue.TechnicianId = ticket.TechnicianId;

            return returnValue;
        }

        // Mapping data to DTO - delete ticket.
        internal static DeleteTicketDto DeleteTicketToDto(Guid ticketId, Guid userId)
        {
            DeleteTicketDto returnValue = new DeleteTicketDto();

            returnValue.UserId = userId;
            returnValue.TicketId = ticketId;

            return returnValue;
        }
    }
}
