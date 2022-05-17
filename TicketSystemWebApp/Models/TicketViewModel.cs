namespace TicketSystemWebApp.Models
{
    public class TicketViewModel
    {
        public Guid TicketID { get; set; }

        public Guid StatusID { get; set; }

        public string StatusName { get; set; } = default!;

        public Guid CategoryID { get; set; }

        public string CategoryName { get; set; } = default!;

        public DateTimeOffset DateTimeCreated { get; set; }

        public DateTimeOffset DateTimeModified { get; set; }

        public string Title { get; set; } = default!;

        public Guid UserID { get; set; }

        public string UserName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public List<MessagesViewModel> Messages { get; set; } = default!;

        public Guid? TechnicianID { get; set; }

        public string? TechnicianName { get; set; }
    }
}
