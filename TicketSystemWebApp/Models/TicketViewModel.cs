namespace TicketSystemWebApp.Models
{
    public class TicketViewModel
    {
        public Guid TicketId { get; set; }

        public int No { get; set; }

        public Guid StatusId { get; set; }

        public string? StatusName { get; set; }

        public Guid CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public DateTimeOffset DateTimeCreated { get; set; }

        public DateTimeOffset DateTimeModified { get; set; }

        public string? Title { get; set; }

        public Guid UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public List<MessagesViewModel>? Messages { get; set; }

        public Guid? TechnicianId { get; set; }

        public string? TechnicianName { get; set; }
    }
}
