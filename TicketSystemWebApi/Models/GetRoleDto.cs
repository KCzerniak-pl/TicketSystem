using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetRoleDto
    {
        [Required]
        public Guid RoleID { get; set; }

        [Required]
        public string RoleName { get; set; } = default!;

        [Required]
        public bool ShowAll { get; set; }

        [Required]
        public bool CanAccepted { get; set; }
    }
}
