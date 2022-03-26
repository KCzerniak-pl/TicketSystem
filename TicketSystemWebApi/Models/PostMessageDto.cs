using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PostMessageDto
    {
        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string Information { get; set; } = default!;
    }
}
