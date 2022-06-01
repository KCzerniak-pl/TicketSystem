using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                IEnumerable<GetTicketsDto> tickets;
                if (userID == default)
                {
                    // Retrieving data from database about all ticket and remapping to DTO.
                    tickets = await _ticketsDbContext.Tickets.OrderByDescending(p => p.DateTimeModified).Skip(skip).Take(take).Include(p => p.Owner).Include(p => p.Category).Include(p => p.Status).Select(p => TicketMapping.GetTicketsToDto(p)).ToArrayAsync();
                }
                else
                {
                    // Retrieving data from database about all ticket for user and remapping to DTO.
                    tickets = await _ticketsDbContext.Tickets.OrderByDescending(p => p.DateTimeModified).Where(p => p.OwnerID == userID).Skip(skip).Take(take).Include(p => p.Owner).Include(p => p.Category).Include(p => p.Status).Select(p => TicketMapping.GetTicketsToDto(p)).ToArrayAsync();
                }

                if (tickets.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, tickets);
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
                int tickets = 0;
                if (userID == default)
                {
                    // Count all ticket in database.
                    tickets = await _ticketsDbContext.Tickets.CountAsync();
                }
                else
                {
                    // Count selected ticket in database.
                    tickets = await _ticketsDbContext.Tickets.Where(p => p.OwnerID == userID).CountAsync();
                }

                return StatusCode(StatusCodes.Status200OK, tickets);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
