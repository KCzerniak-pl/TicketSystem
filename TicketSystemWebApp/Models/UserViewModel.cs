namespace TicketSystemWebApp.Models
{
    public class UserViewModel
    {
        public Guid UserID { get; set; }

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public RoleViewModel Role { get; set; } = default!;

        public string DateTimeCreated { get; set; } = default!;
    }
}
