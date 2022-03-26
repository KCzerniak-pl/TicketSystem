using System.ComponentModel.DataAnnotations;

namespace TicketSystemWebApi.Models
{
    public class GetCategoriesDto
    {
        [Required]
        public Guid CategoryID { get; set; }

        [Required]
        public string Name { get; set; } = default!;
    }
}
