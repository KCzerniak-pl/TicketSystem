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

            returnValue.TicketID = ticket.TicketID;
            returnValue.StatusID = ticket.Status.StatusID;
            returnValue.StatusName = ticket.Status.Name;
            returnValue.CategoryID = ticket.CategoryID;
            returnValue.CategoryName = ticket.Category.Name;
            returnValue.DateTimeCreated = ticket.DateTimeCreated;
            returnValue.DateTimeModified = ticket.DateTimeModified;
            returnValue.Title = ticket.Title;
            returnValue.UserID = ticket.Owner.UserID;
            returnValue.UserName = String.Format("{0} {1}", ticket.Owner.FirstName, ticket.Owner.LastName);
            returnValue.Email = ticket.Owner.Email;

            List<GetMessagesDto> messages = new List<GetMessagesDto>();
            messages.AddRange(ticket.Messages.Where(p => p.TicketID == ticket.TicketID).Select(p => MessageMapping.GetMessageToDto(p)).OrderBy(p => p.DateTimeCreated));
            returnValue.Messages = messages;

            return returnValue;
        }

        // Mapping data from database to DTO - tickets list.
        internal static GetTicketsDto GetTicketsToDto(Database.Entities.Ticket ticket)
        {
            GetTicketsDto returnValue = new GetTicketsDto();

            returnValue.TicketID = ticket.TicketID;
            returnValue.StatusID = ticket.Status.StatusID;
            returnValue.StatusName = ticket.Status.Name;
            returnValue.CategoryName = ticket.Category.Name;
            returnValue.DateTimeCreated = ticket.DateTimeCreated;
            returnValue.DateTimeModified = ticket.DateTimeModified;
            returnValue.Title = ticket.Title;
            returnValue.UserID = ticket.OwnerID;
            returnValue.UserName = String.Format("{0} {1}", ticket.Owner.FirstName, ticket.Owner.LastName);

            return returnValue;
        }

        // Mapping DTO to data save in database - new ticket.
        internal static Database.Entities.Ticket PostTicketFromDto(PostTicketDto ticket, Guid statusForNewTicket)
        {
            Database.Entities.Ticket returnValue = new Database.Entities.Ticket();

            returnValue.TicketID = Guid.NewGuid();
            returnValue.OwnerID = ticket.UserID;
            returnValue.StatusID = statusForNewTicket;
            returnValue.CategoryID = ticket.CategoryID;
            returnValue.DateTimeCreated = DateTime.Now;
            returnValue.DateTimeModified = returnValue.DateTimeCreated;
            returnValue.Title = ticket.Title;

            Database.Entities.Message message = new Database.Entities.Message()
            {
                MessageID = Guid.NewGuid(),
                TicketID = returnValue.TicketID,
                OwnerID = ticket.UserID,
                Information = ticket.Information,
                DateTimeCreated = returnValue.DateTimeCreated
            };

            returnValue.Messages = new Collection<Database.Entities.Message>() { message };

            return returnValue;
        }

        // Mapping DTO to data update in database - status.
        internal static Database.Entities.Ticket PutTicketStatusFromDto(Database.Entities.Ticket returnValue, PutTicketStatusDto ticket)
        {
            returnValue.StatusID = ticket.StatusID;
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
            returnValue.CategoryID = ticket.CategoryID;
            returnValue.DateTimeModified = DateTime.Now;

            return returnValue;
        }
    }
}
