using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetRoleDto
    {
        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string? RoleName { get; set; }

        [Required]
        public bool ShowAll { get; set; }

        [Required]
        public bool CanAccepted { get; set; }

        [Required]
        public bool Technician { get; set; }
    }
}
