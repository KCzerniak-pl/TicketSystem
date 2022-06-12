using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketTitleDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public string? Title { get; set; }
    }
}
