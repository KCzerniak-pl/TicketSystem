using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid TicketId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int No { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid OwnerId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? TechnicianId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid StatusId { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid CategoryId { get; set; }

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeModified { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        [Required]
        public string? Title { get; set; }

        // Relationships one-to-many.
        public virtual User? Owner { get; set; }
        public virtual User? Technician { get; set; }
        public virtual Status? Status { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
    }
}
