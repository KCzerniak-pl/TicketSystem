using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid CategoryID { get; set; } = default!;

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string Name { get; set; } = default!;

        // Relationships one-to-many.
        public ICollection<Ticket> Tickets { get; set; } = default!;
    }
}
