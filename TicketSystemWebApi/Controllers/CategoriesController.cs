using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemWebApi.Mapping;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        // Required references to the library "Database".
        private readonly CategoriesDbContext _categoriesDbContext;

        // Required configuration of connection context with the database in the Program.cs file.
        public CategoriesController(CategoriesDbContext categoriesDbContext)
        {
            _categoriesDbContext = categoriesDbContext;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoriesDto>>> GetCategories()
        {
            if (_categoriesDbContext.Database.CanConnect())
            {
                // Retrieving data from database about all categories and remapping to DTO.
                IEnumerable<GetCategoriesDto> result = await _categoriesDbContext.Categories.Select(p => CategoryMapping.GetCategoriesToDto(p)).ToArrayAsync();

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
