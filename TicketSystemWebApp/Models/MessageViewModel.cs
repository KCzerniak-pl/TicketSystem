namespace TicketSystemWebApp.Models
{
    public class MessagesViewModel
    {
        public Guid MessageID { get; set; }

        public string Information { get; set; } = default!;

        public DateTimeOffset DateTimeCreated { get; set; }

        public Guid UserID { get; set; }

        public string UserName { get; set; } = default!;
    }
}
