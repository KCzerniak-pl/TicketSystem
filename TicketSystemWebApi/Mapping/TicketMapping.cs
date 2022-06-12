using System.Collections.ObjectModel;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Mapping
{
    public class TicketMapping
    {
        // Mapping data from database to DTO - selected ticket.
        internal static GetTicketDto GetTicketToDto(Database.Entities.Ticket ticket)
        {
            GetTicketDto returnValue = new GetTicketDto();

            returnValue.TicketId = ticket.TicketId;
            returnValue.StatusId = ticket.Status!.StatusId;
            returnValue.StatusName = ticket.Status.Name;
            returnValue.CategoryId = ticket.CategoryId;
            returnValue.CategoryName = ticket.Category!.Name;
            returnValue.DateTimeCreated = ticket.DateTimeCreated;
            returnValue.DateTimeModified = ticket.DateTimeModified;
            returnValue.Title = ticket.Title;
            returnValue.UserId = ticket.Owner!.UserId;
            returnValue.UserName = String.Format("{0} {1}", ticket.Owner.FirstName, ticket.Owner.LastName);
            returnValue.Email = ticket.Owner.Email;
            returnValue.TechnicianId = ticket.TechnicianId;

            List<GetMessagesDto> messages = new List<GetMessagesDto>();
            messages.AddRange(ticket.Messages!.Where(p => p.TicketId == ticket.TicketId).Select(p => MessageMapping.GetMessageToDto(p)).OrderBy(p => p.DateTimeCreated));
            returnValue.Messages = messages;

            return returnValue;
        }

        // Mapping data from database to DTO - tickets list.
        internal static GetTicketsDto GetTicketsToDto(Database.Entities.Ticket ticket)
        {
            GetTicketsDto returnValue = new GetTicketsDto();

            returnValue.TicketId = ticket.TicketId;
            returnValue.No = ticket.No;
            returnValue.StatusId = ticket.Status!.StatusId;
            returnValue.StatusName = ticket.Status.Name!;
            returnValue.CategoryName = ticket.Category!.Name!;
            returnValue.DateTimeCreated = ticket.DateTimeCreated;
            returnValue.DateTimeModified = ticket.DateTimeModified;
            returnValue.Title = ticket.Title!;
            returnValue.UserId = ticket.OwnerId;
            returnValue.UserName = String.Format("{0} {1}", ticket.Owner!.FirstName, ticket.Owner!.LastName);

            return returnValue;
        }

        // Mapping DTO to data save in database - new ticket.
        internal static Database.Entities.Ticket PostTicketFromDto(PostTicketDto ticket, Guid statusForNewTicket)
        {
            Database.Entities.Ticket returnValue = new Database.Entities.Ticket();

            returnValue.TicketId = Guid.NewGuid();
            returnValue.OwnerId = ticket.UserId;
            returnValue.StatusId = statusForNewTicket;
            returnValue.CategoryId = ticket.CategoryId;
            returnValue.DateTimeCreated = DateTime.Now;
            returnValue.DateTimeModified = returnValue.DateTimeCreated;
            returnValue.Title = ticket.Title;

            Database.Entities.Message message = new Database.Entities.Message()
            {
                MessageId = Guid.NewGuid(),
                TicketId = returnValue.TicketId,
                OwnerId = ticket.UserId,
                Information = ticket.Information,
                DateTimeCreated = returnValue.DateTimeCreated
            };

            returnValue.Messages = new Collection<Database.Entities.Message>() { message };

            return returnValue;
        }

        // Mapping DTO to data update in database - status.
        internal static Database.Entities.Ticket PutTicketStatusFromDto(Database.Entities.Ticket returnValue, PutTicketStatusDto ticket)
        {
            returnValue.StatusId = ticket.StatusId;
            returnValue.TechnicianId = ticket.TechnicianId;
            returnValue.DateTimeModified = DateTime.Now;

            return returnValue;
        }

        // Mapping DTO to data update in database - title.
        internal static Database.Entities.Ticket PutTicketTitleFromDto(Database.Entities.Ticket returnValue, PutTicketTitleDto ticket)
        {
            returnValue.Title = ticket.Title;
            returnValue.DateTimeModified = DateTime.Now;

            return returnValue;
        }

        // Mapping DTO to data update in database - category.
        internal static Database.Entities.Ticket PutTicketCategoryFromDto(Database.Entities.Ticket returnValue, PutTicketCategoryDto ticket)
        {
            returnValue.CategoryId = ticket.CategoryId;
            returnValue.DateTimeModified = DateTime.Now;

            return returnValue;
        }

        // Mapping DTO to data update in database - technician.
        internal static Database.Entities.Ticket PutTicketTechnicianFromDto(Database.Entities.Ticket returnValue, PutTicketTechnicianDto ticket)
        {
            returnValue.TechnicianId = ticket.TechnicianId;
            returnValue.DateTimeModified = DateTime.Now;

            return returnValue;
        }
    }
}
