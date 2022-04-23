using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly TicketsDbContext _ticketsDbContext;

        // Required configuration of connection context with the database in the Program.cs file and references to the library "Database".
        public TicketsController(TicketsDbContext ticketsDbContext)
        {
            _ticketsDbContext = ticketsDbContext;
        }

        //GET: api/<ValuesController>?skip=<skip>&take=<take>&userID=<userID>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketsDto>>> GetTickets([FromQuery, BindRequired] int skip, [FromQuery, BindRequired] int take, [FromQuery] Guid userID = default)
        {
            if (_ticketsDbContext.Database.CanConnect())
            {
                IEnumerable<GetTicketsDto> result;
                if (userID == default)
                {
                    // Retrieving data from database about all ticket and remapping to DTO.
                    result = await _ticketsDbContext.Tickets.OrderByDescending(p => p.DateTimeCreated).Skip(skip).Take(take).Include(p => p.User).Include(p => p.Category).Include(p => p.Status).Select(p => TicketMapping.GetTicketsToDto(p)).ToArrayAsync();
                }
                else
                {
                    // Retrieving data from database about all ticket for user and remapping to DTO.
                    result = await _ticketsDbContext.Tickets.OrderByDescending(p => p.DateTimeCreated).Where(p => p.UserID == userID).Skip(skip).Take(take).Include(p => p.User).Include(p => p.Category).Include(p => p.Status).Select(p => TicketMapping.GetTicketsToDto(p)).ToArrayAsync();
                }

                if (result.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //GET: api/<ValuesController>/Count?userID=<userID>
        [HttpGet("Count")]
        public async Task<ActionResult<int>> GetTicketsCount([FromQuery] Guid userID = default)
        {
            if (_ticketsDbContext.Database.CanConnect())
            {
                int result = 0;
                if (userID == default)
                {
                    // Count all ticket in database.
                    result = await _ticketsDbContext.Tickets.CountAsync();
                }
                else
                {
                    // Count selected ticket in database.
                    result = await _ticketsDbContext.Tickets.Where(p => p.UserID == userID).CountAsync();
                }

                return StatusCode(StatusCodes.Status200OK, result);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
