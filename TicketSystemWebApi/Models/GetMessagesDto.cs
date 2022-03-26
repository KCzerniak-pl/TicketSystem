using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetMessagesDto
    {
        [Required]
        public Guid MessageID { get; set; }

        [Required]
        public string Information { get; set; } = default!;

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string UserName { get; set; } = default!;
    }
}
