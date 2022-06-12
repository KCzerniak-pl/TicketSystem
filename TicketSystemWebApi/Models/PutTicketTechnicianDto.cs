using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketTechnicianDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid TechnicianId { get; set; }
    }
}
