using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Messages")]
    public class Message
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid MessageID { get; set; } = default!;

        [Required]
        public Guid TicketID { get; set; } = default!;

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid OwnerID { get; set; } = default!;

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string Information { get; set; } = default!;

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeCreated { get; set; } = default!;

        // Relationships one-to-many.
        public Ticket Ticket { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
