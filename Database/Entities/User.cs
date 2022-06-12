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
        public Guid UserId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string? Email { get; set; }

        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid RoleId { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Required]
        public string? PasswordHash { get; set; }

        [Column(TypeName = "datetimeoffset(0)")]
        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }
        
        // Relationships one-to-many.
        public UserRole? Role { get; set; }
        public ICollection<Message>? Messages { get; set; }

        [InverseProperty("Owner")]
        public virtual ICollection<Ticket>? OwnerTickets { get; set; }

        [InverseProperty("Technician")]
        public virtual ICollection<Ticket>? TechnicianTickets { get; set; }
    }
}
