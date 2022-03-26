using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PostTicketDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        public string Subject { get; set; } = default!;

        [Required]
        public string Information { get; set; } = default!;
    }
}
