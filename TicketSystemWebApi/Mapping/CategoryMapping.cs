using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Mapping
{
    public class CategoryMapping
    {
        // Mapping data from database to DTO - categories list.
        internal static GetCategoriesDto GetCategoriesToDto(Database.Entities.Category category)
        {
            GetCategoriesDto returnValue = new GetCategoriesDto();

            returnValue.CategoryId = category.CategoryId;
            returnValue.Name = category.Name;

            return returnValue;
        }
    }
}
