using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetTicketsDto
    {
        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public int No { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        [Required]
        public string? StatusName { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        [Required]
        public DateTimeOffset DateTimeModified { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string? UserName { get; set; }
    }
}
