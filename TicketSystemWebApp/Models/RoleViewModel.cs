namespace TicketSystemWebApp.Models
{
    public class RoleViewModel
    {
        public Guid RoleId { get; set; }

        public string? RoleName { get; set; }

        public bool ShowAll { get; set; }

        public bool CanAccepted { get; set; }
    }
}
