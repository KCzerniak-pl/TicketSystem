namespace TicketSystemWebApp.Models
{
    public class RoleViewModel
    {
        public Guid RoleID { get; set; }

        public string RoleName { get; set; } = default!;

        public bool ShowAll { get; set; }

        public bool CanAccepted { get; set; }
    }
}
