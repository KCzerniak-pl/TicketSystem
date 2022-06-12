using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetTicketDto
    {
        [Required]
        public Guid TicketId { get; set; }

        [Required]
        public Guid StatusId { get; set; }

        [Required]
        public string? StatusName { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

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

        [Required]
        public string? Email { get; set; }

        public List<GetMessagesDto>? Messages { get; set; }

        public Guid? TechnicianId { get; set; }
    }
}
