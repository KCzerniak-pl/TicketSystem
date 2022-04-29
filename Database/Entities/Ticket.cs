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
        public Guid TicketID { get; set; } = default!;

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid OwnerID { get; set; } = default!;

        [Column(TypeName = "uniqueidentifier")]
        public Guid? TechnicianID { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid StatusID { get; set; } = default!;

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid CategoryID { get; set; } = default!;

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeCreated { get; set; } = default!;

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeModified { get; set; } = default!;

        [Column(TypeName = "nvarchar(75)")]
        [Required]
        public string Title { get; set; } = default!;

        // Relationships one-to-many.
        public User OwnerUser { get; set; } = default!;
        public User TechnicianUser { get; set; } = default!;
        public Status Status { get; set; } = default!;
        public Category Category { get; set; } = default!;
        public ICollection<Message> Messages { get; set; } = default!;
    }
}
