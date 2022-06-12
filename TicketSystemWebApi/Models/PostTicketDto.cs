using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PostTicketDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Information { get; set; }
    }
}
