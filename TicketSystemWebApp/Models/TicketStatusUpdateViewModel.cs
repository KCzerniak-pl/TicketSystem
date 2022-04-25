using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class TicketStatusUpdateViewModel
    {
        public Guid TicketID { get; set; }

        public Guid StatusID { get; set; }
    }
}
