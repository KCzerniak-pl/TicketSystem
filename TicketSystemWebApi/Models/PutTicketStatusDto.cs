using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketStatusDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        public Guid TechnicianId { get; set; }
    }
}
