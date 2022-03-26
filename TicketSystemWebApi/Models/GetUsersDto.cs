using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetUsersDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;

        [Required]
        public string Email { get; set; } = default!;

        [Required]
        public GetRoleDto Role { get; set; } = default!;

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }
    }
}
