namespace TicketSystemWebApp.Models
{
    public class MessagesViewModel
    {
        public Guid MessageId { get; set; }

        public string? Information { get; set; }

        public DateTimeOffset DateTimeCreated { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }
    }
}
