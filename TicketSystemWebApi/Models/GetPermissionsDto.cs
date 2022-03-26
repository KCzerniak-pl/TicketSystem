using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetPermissionsDto
    {
        [Required]
        public bool ShowAll { get; set; }

        [Required]
        public bool CanAccepted { get; set; }
    }
}
