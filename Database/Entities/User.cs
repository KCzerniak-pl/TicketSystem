using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid UserID { get; set; } = default!;

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string FirstName { get; set; } = default!;

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string LastName { get; set; } = default!;

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Email { get; set; } = default!;

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid RoleID { get; set; } = default!;

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string PasswordHash { get; set; } = default!;

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeCreated { get; set; } = default!;

        // Relationships one-to-many.
        public UserRole Role { get; set; } = default!;
        public ICollection<Ticket> Tickets { get; set; } = default!;
        public ICollection<Message> Messages { get; set; } = default!;
    }
}
