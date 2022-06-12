using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetUsersDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public GetRoleDto? Role { get; set; }

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }
    }
}
