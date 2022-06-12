using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class MessageNewViewModel
    {
        public Guid TicketId { get; set; }

        public string? Information { get; set; }
    }
}
