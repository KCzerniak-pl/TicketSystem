using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetTicketDto
    {
        [Required]
        public Guid TicketID { get; set; }

        [Required]
        public Guid StatusID { get; set; }

        [Required]
        public string StatusName { get; set; } = default!;

        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; } = default!;

        [Required]
        public DateTimeOffset DateTimeCreated { get; set; }

        [Required]
        public DateTimeOffset DateTimeModified { get; set; }

        [Required]
        public string Subject { get; set; } = default!;

        [Required]
        public Guid UserID { get; set; }

        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        public string Email { get; set; } = default!;

        public List<GetMessagesDto> Messages { get; set; } = default!;

        [Required]
        public GetPermissionsDto Permission { get; set; } = default!;
    }
}
