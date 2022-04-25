using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; } = default!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        public bool RemeberMe { get; set; }
    }
}
