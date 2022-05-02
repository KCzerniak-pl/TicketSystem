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
    public class MessageController : ControllerBase
    {
        private readonly TicketsDbContext _ticketsDbContext;
        private readonly UsersDbContext _usersDbContext;
        private readonly MessagesDbContext _messagesDbContext;

        // Required configuration of connection context with the database in the Program.cs file and references to the library "Database".
        public MessageController(TicketsDbContext tickeetsDbContext, UsersDbContext usersDbContext, MessagesDbContext messagesDbContext)
        {
            _ticketsDbContext = tickeetsDbContext;
            _usersDbContext = usersDbContext;
            _messagesDbContext = messagesDbContext;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] PostMessageDto postMessage)
        {
            if (_messagesDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == postMessage.TicketID).Include(p => p.Owner).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == postMessage.UserID).FirstAsync();

                        // Verification that user can add new message for ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerID == postMessage.UserID || user.Role.ShowAll == true)
                        {
                            // Add new massage for ticket.
                            _ = _messagesDbContext.Messages.Add(MessageMapping.PostMessageFromDto(postMessage));
                            _ = await _messagesDbContext.SaveChangesAsync();

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
