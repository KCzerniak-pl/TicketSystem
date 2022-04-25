namespace TicketSystemWebApp.Models
{
    public class TicketTitleUpdateViewModel
    {
        public Guid TicketID { get; set; }

        public string Title { get; set; } = default!;
    }
}
