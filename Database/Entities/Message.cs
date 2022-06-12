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
        public Guid MessageId { get; set; }

        [Required]
        public Guid TicketId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid OwnerId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string? Information { get; set; }

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        // Relationships one-to-many.
        public virtual Ticket? Ticket { get; set; }

        public virtual User? Owner { get; set; }
    }
}
