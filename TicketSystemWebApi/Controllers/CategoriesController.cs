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
        private readonly CategoriesDbContext _dbContext;

        public CategoriesController(CategoriesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoriesDto>>> GetCategories()
        {
            if (_dbContext.Database.CanConnect())
            {
                // Retrieving data from database about all categories and remapping to DTO.
                // Required configuration of connection context with the database in the Startup.cs file.
                IEnumerable<GetCategoriesDto> result = await _dbContext.Categories.Select(p => CategoryMapping.GetCategoriesToDto(p)).ToArrayAsync();

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
