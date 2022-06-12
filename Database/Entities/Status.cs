using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Statuses")]
    public class Status
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid StatusId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? Name { get; set; }

        // Relationships one-to-many.
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
