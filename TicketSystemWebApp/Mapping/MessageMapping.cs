using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class MessageMapping
    {
        // Mapping DTO to object used by the application - messages list.
        internal static MessagesViewModel GetMessagesFromDto(GetMessagesDto message)
        {
            MessagesViewModel returnValue = new MessagesViewModel();

            returnValue.MessageId = message.MessageId;
            returnValue.UserId = message.UserId;
            returnValue.UserName = message.UserName;
            returnValue.Information = message.Information;
            returnValue.DateTimeCreated = message.DateTimeCreated;

            return returnValue;
        }

        // Mapping data to DTO - new message.
        internal static PostMessageDto PostMessageToDto(MessageNewViewModel message, Guid userId)
        {
            PostMessageDto returnValue = new PostMessageDto();

            returnValue.UserId = userId;
            returnValue.TicketId = message.TicketId;
            returnValue.Information = message.Information;

            return returnValue;
        }
    }
}
