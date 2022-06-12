using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class TicketStatusUpdateViewModel
    {
        public Guid TicketId { get; set; }

        public Guid StatusId { get; set; }
    }
}
