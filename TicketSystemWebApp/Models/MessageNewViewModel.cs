using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class MessageNewViewModel
    {
        public Guid TicketID { get; set; }

        public string Information { get; set; } = default!;
    }
}
