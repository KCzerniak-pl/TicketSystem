using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class DeleteTicketDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

    }
}
