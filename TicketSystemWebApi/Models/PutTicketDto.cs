using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketDto
    {
        [Required]
        public Guid TicketID { get; set; }

        public Guid StatusID { get; set; }

        public Guid CategoryID { get; set; }

        public string Subject { get; set; } = default!;
    }
}
