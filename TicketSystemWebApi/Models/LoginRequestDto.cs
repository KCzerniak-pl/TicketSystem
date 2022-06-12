using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
