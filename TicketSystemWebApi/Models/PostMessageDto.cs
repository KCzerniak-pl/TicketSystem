using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PostMessageDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public string? Information { get; set; }
    }
}
