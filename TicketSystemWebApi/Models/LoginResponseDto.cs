using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class LoginResponseDto
    {
        [Required]
        public bool Success { get; set; } = default!;

        [Required]
        public Guid UserID { get; set; } = default!;

        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        public string Error { get; set; } = default!;
    }
}
