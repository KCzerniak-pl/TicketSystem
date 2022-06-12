using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemWebApi.Mapping;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageController : ControllerBase
    {
        private readonly TicketSystemDbContext _ticketSystemDbContext;

        // Required configuration of connection context with the database in the Program.cs file and references to the library "Database".
        public MessageController(TicketSystemDbContext ticketSystemDbContext)
        {
            _ticketSystemDbContext = ticketSystemDbContext;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] PostMessageDto postMessage)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == postMessage.TicketId).Include(p => p.Owner).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == postMessage.UserId).Include(p => p.Role).FirstAsync();

                        // Verification that user can add new message for ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerId == postMessage.UserId || user.Role!.ShowAll == true)
                        {
                            // Add new massage for ticket.
                            _ = _ticketSystemDbContext.Messages!.Add(MessageMapping.PostMessageFromDto(postMessage));

                            // Set date update for ticket.
                            ticket.DateTimeModified = DateTime.Now;

                            _ = await _ticketSystemDbContext.SaveChangesAsync();

                            return StatusCode(StatusCodes.Status204NoContent);
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status403Forbidden);
                        }
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
