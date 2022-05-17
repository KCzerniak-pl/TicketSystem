using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketTechnicianDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public Guid TechnicianID { get; set; }
    }
}
