using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetMessagesDto
    {
        [Required]
        public Guid MessageId { get; set; }

        [Required]
        public string? Information { get; set; }

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string? UserName { get; set; }
    }
}
