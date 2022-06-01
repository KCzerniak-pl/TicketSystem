using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class LoginResponseDto
    {
        [Required]
        public string Jwt { get; set; } = default!;

        [Required]
        public bool Success { get; set; } = default!;

        [Required]
        public string Error { get; set; } = default!;
    }
}
