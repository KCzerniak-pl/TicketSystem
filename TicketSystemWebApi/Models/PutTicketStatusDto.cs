using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketStatusDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public Guid StatusID { get; set; }
    }
}
