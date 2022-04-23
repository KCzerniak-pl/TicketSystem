using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketTitleDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public string Title { get; set; } = default!;
    }
}
