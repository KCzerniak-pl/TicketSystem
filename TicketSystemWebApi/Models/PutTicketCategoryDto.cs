using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketCategoryDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
