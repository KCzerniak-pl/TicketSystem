using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class DeleteTicketDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid TicketID { get; set; }

    }
}
