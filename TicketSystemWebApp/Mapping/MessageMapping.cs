using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class MessageMapping
    {
        // Mapping DTO to object used by the application - messages list.
        internal static MessagesViewModel GetMessagesFromDto(GetMessagesDto message)
        {
            MessagesViewModel returnValue = new MessagesViewModel();

            returnValue.MessageID = message.MessageID;
            returnValue.UserID = message.UserID;
            returnValue.UserName = message.UserName;
            returnValue.Information = message.Information;
            returnValue.DateTimeCreated = message.DateTimeCreated;

            return returnValue;
        }

        // Mapping data to DTO - new message.
        internal static PostMessageDto PostMessageToDto(MessageNewViewModel message, Guid userID)
        {
            PostMessageDto returnValue = new PostMessageDto();

            returnValue.UserID = userID;
            returnValue.TicketID = message.TicketID;
            returnValue.Information = message.Information;

            return returnValue;
        }
    }
}
