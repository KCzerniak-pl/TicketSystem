using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class LoginResponseDto
    {
        [Required]
        public string? Jwt { get; set; }

        [Required]
        public bool Success { get; set; }

        [Required]
        public string? Error { get; set; }
    }
}
