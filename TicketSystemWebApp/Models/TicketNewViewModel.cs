 using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class TicketNewViewModel
    {
        public Guid CategoryId { get; set; }

        public string? Title { get; set; }

        public string? Information { get; set; }
    }
}
