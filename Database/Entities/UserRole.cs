using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        [Required]
        public Guid RoleId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? RoleName { get; set; }

        [Required]
        public bool ShowAll { get; set; }

        [Required]
        public bool CanAccepted { get; set; }

        [Required]
        public bool Technician { get; set; }

        // Relationships one-to-many.
        public virtual ICollection<User>? Users { get; set; }
    }
}
