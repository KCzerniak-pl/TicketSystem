using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Mapping
{
    public class MessageMapping
    {
        // Mapping data from database to DTO - messages list.
        internal static GetMessagesDto GetMessageToDto(Database.Entities.Message message)
        {
            GetMessagesDto returnValue = new GetMessagesDto();

            returnValue.MessageId = message.MessageId;
            returnValue.Information = message.Information;
            returnValue.DateTimeCreated = message.DateTimeCreated;
            returnValue.UserId = message.Owner!.UserId;
            returnValue.UserName = String.Format("{0} {1}", message.Owner!.FirstName, message.Owner!.LastName);

            return returnValue;
        }

        // Mapping DTO to data save in database - new message
        internal static Database.Entities.Message PostMessageFromDto(PostMessageDto message)
        {
            Database.Entities.Message returnValue = new Database.Entities.Message();

            returnValue.MessageId = Guid.NewGuid();
            returnValue.TicketId = message.TicketId;
            returnValue.OwnerId = message.UserId;
            returnValue.Information = message.Information;
            returnValue.DateTimeCreated = DateTime.Now;

            return returnValue;
        }
    }
}
