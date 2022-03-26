using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemWebApi.Mapping;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TicketsController : ControllerBase
    {
        // Required references to the library "Database".
        private readonly TicketsDbContext _dbContext;

        public TicketsController(TicketsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET: api/<ValuesController>/<userID>
        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<GetTicketsDto>>> GetTicketsForUser([FromRoute] Guid userID)
        {
            if (_dbContext.Database.CanConnect())
            {
                // Retrieving data from database about selected ticket and remapping to DTO.
                // Required configuration of connection context with the database in the Startup.cs file.
                IEnumerable<GetTicketsDto> result = await _dbContext.Tickets.Where(p => p.UserID == userID).Include(p => p.User).Include(p => p.Category).Include(p => p.Status).Select(p => TicketMapping.GetTicketsToDto(p)).ToListAsync();

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, result.OrderByDescending(p => p.DateTimeCreated));
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketsDto>>> GetTickets()
        {
            if (_dbContext.Database.CanConnect())
            {
                // Retrieving data from database about selected ticket and remapping to DTO.
                // Required configuration of connection context with the database in the Startup.cs file.
                IEnumerable<GetTicketsDto> result = await _dbContext.Tickets.Include(p => p.User).Include(p => p.Category).Include(p => p.Status).Select(p => TicketMapping.GetTicketsToDto(p)).ToListAsync();

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, result.OrderByDescending(p => p.DateTimeCreated));
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
