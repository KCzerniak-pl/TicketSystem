using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApp.Models
{
    public class LoginViewModel
    {
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RemeberMe { get; set; }
    }
}
