namespace TicketSystemWebApp.Models
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public RoleViewModel? Role { get; set; }

        public string? DateTimeCreated { get; set; }
    }
}
