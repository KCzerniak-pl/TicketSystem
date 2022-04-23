using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class PutTicketCategoryDto
    {
        [Required]
        public Guid UserID { get; set; }

        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public Guid CategoryID { get; set; }
    }
}
