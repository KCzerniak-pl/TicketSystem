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
        public Guid RoleID { get; set; } = default!;

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string RoleName { get; set; } = default!;

        [Required]
        public bool ShowAll { get; set; } = default!;

        [Required]
        public bool CanAccepted { get; set; } = default!;

        [Required]
        public bool Technician { get; set; } = default!;

        // Relationships one-to-many.
        public ICollection<User> Users { get; set; } = default!;
    }
}
