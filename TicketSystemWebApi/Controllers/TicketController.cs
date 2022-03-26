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
    public class TicketController : ControllerBase
    {
        // Required references to the library "Database".
        private readonly TicketsDbContext _ticketsDbContext;
        private readonly UsersDbContext _usersDbContext;

        public TicketController(TicketsDbContext tickeetsDbContext, UsersDbContext usersDbContext)
        {
            _ticketsDbContext = tickeetsDbContext;
            _usersDbContext = usersDbContext;
        }

        //GET: api/<ValuesController>?ticketID=<ticketID>&userID=<UserID>
        [HttpGet]
        public async Task<ActionResult<GetTicketDto>> GetTicketByID([FromQuery] Guid ticketID, [FromQuery] Guid userID)
        {
            if (_ticketsDbContext.Database.CanConnect())
            {
                // Retrieving data from database about selected user.
                // Required configuration of connection context with the database in the Startup.cs file.
                var userResult = await _usersDbContext.Users.Where(p => p.UserID == userID).Include(p => p.Role).FirstOrDefaultAsync();

                // Retrieving data from database about selected ticket and remapping to DTO.
                // Required configuration of connection context with the database in the Startup.cs file.
                GetTicketDto? result = await _ticketsDbContext.Tickets.Where(p => p.TicketID == ticketID).Include(p => p.User).Include(p => p.Category).Include(p => p.Status).Include(p => p.Messages).ThenInclude(p => p.User).Select(p => TicketMapping.GetTicketToDto(p)).FirstOrDefaultAsync();

                // Verification that the ticket has been found and that user can view it  (must be its author or have permission to view all tickets).
                if (result != null && (result.UserID == userID || userResult?.Role.ShowAll == true))
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromBody] PostTicketDto ticket)
        {
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    // Add new ticket.
                    // Required configuration of connection context with the database in the Startup.cs file.
                    _ticketsDbContext.Tickets.Add(TicketMapping.PostTicketFromDto(ticket));
                    await _ticketsDbContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status204NoContent);
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PUT api/<ValuesController>
        [HttpPut]
        public async Task<IActionResult> PutTicket([FromBody] PutTicketDto ticket)
        {
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    // Pobranie z bazy danych informacji o wybranym zgłoszeniu.
                    // Wymaga dodatkowej konfiguracji kontekstu połączenia z bazą w pliku Startup.cs.
                    var result = await _ticketsDbContext.Tickets.Where(p => p.TicketID == ticket.TicketID).FirstOrDefaultAsync();

                    if (result != null)
                    {
                        // Aktulizacja informacji o wybranym zgłoszeniu.
                        result = TicketMapping.PutTicketFromDto(result, ticket);

                        // Akutalizacja w bazie danych informacji o wybranym zgłoszeniu.
                        // Wymaga dodatkowej konfiguracji kontekstu połączenia z bazą w pliku Startup.cs.
                        await _ticketsDbContext.SaveChangesAsync();

                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
