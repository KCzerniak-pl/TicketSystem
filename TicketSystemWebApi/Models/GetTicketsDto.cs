using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetTicketsDto
    {
        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public int No { get; set; }

        [Required]
        public Guid StatusID { get; set; }

        [Required]
        public string StatusName { get; set; } = default!;

        [Required]
        public string CategoryName { get; set; } = default!;

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        [Required]
        public DateTimeOffset DateTimeModified { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string UserName { get; set; } = default!;
    }
}
