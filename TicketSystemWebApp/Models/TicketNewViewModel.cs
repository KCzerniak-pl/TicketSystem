 using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class TicketNewViewModel
    {
        public Guid CategoryID { get; set; }

        public string Title { get; set; } = default!;

        public string Information { get; set; } = default!;
    }
}
