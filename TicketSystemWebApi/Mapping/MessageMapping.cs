using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Mapping
{
    public class MessageMapping
    {
        // Mapping data from database to DTO - messages list.
        internal static GetMessagesDto GetMessageToDto(Database.Entities.Message message)
        {
            GetMessagesDto returnValue = new GetMessagesDto();

            returnValue.MessageID = message.MessageID;
            returnValue.Information = message.Information;
            returnValue.DateTimeCreated = message.DateTimeCreated;
            returnValue.UserID = message.User.UserID;
            returnValue.UserName = String.Format("{0} {1}", message.User.FirstName, message.User.LastName);

            return returnValue;
        }

        // Mapping DTO to data save in database - new message
        internal static Database.Entities.Message PostMessageFromDto(PostMessageDto message)
        {
            Database.Entities.Message returnValue = new Database.Entities.Message();

            returnValue.MessageID = Guid.NewGuid();
            returnValue.TicketID = message.TicketID;
            returnValue.OwnerID = message.UserID;
            returnValue.Information = message.Information;
            returnValue.DateTimeCreated = DateTime.Now;

            return returnValue;
        }
    }
}
