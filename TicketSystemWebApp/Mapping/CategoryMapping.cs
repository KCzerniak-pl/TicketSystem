using TicketSystemWebApp.Models;

namespace TicketSystemWebApp.Mapping
{
    public class CategoryMapping
    {
        // Mapping DTO to object used by the application - categories.
        internal static CategoryViewModel GetCategoriesFromDto(GetCategoriesDto dto)
        {
            CategoryViewModel returnValue = new CategoryViewModel();

            returnValue.CategoryId = dto.CategoryId;
            returnValue.Name = dto.Name;

            return returnValue;
        }
    }
}
