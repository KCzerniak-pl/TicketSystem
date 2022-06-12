using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetCategoriesDto
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
