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
            returnValue.Subject = ticket.Subject;
            returnValue.UserID = ticket.User.UserID;
            returnValue.UserName = String.Format("{0} {1}", ticket.User.FirstName, ticket.User.LastName);
            returnValue.Email = ticket.User.Email;

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
            returnValue.Subject = ticket.Subject;
            returnValue.UserID = ticket.UserID;
            returnValue.UserName = String.Format("{0} {1}", ticket.User.FirstName, ticket.User.LastName);

            return returnValue;
        }

        // Mapping DTO to data save in database - new ticket.
        internal static Database.Entities.Ticket PostTicketFromDto(PostTicketDto ticket)
        {
            Database.Entities.Ticket returnValue = new Database.Entities.Ticket();

            returnValue.TicketID = Guid.NewGuid();
            returnValue.UserID = ticket.UserID;
            returnValue.StatusID = new Guid("0b12f12f-d6ff-4a90-8f73-c26303ce5a7d");
            returnValue.CategoryID = ticket.CategoryID;
            returnValue.DateTimeCreated = DateTime.Now;
            returnValue.DateTimeModified = returnValue.DateTimeCreated;
            returnValue.Subject = ticket.Subject;

            Database.Entities.Message message = new Database.Entities.Message()
            {
                MessageID = Guid.NewGuid(),
                TicketID = returnValue.TicketID,
                UserID = ticket.UserID,
                Information = ticket.Information,
                DateTimeCreated = returnValue.DateTimeCreated
            };

            returnValue.Messages = new Collection<Database.Entities.Message>() { message };

            return returnValue;
        }

        // Mapping DTO to data update in database - selected ticket.
        internal static Database.Entities.Ticket PutTicketFromDto(Database.Entities.Ticket returnValue, PutTicketDto ticket)
        {

            if (ticket.StatusID != Guid.Empty)
            {
                returnValue.StatusID = ticket.StatusID;
            }
            else if (ticket.CategoryID != Guid.Empty)
            {
                returnValue.CategoryID = ticket.CategoryID;
            }
            else if (ticket.Subject != null)
            {
                returnValue.Subject = ticket.Subject;
            }

            returnValue.DateTimeModified = DateTime.Now;

            return returnValue;
        }
    }
}
