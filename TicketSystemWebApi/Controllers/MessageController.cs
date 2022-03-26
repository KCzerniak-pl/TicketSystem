using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketSystemWebApi.Mapping;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MessageController : ControllerBase
    {
        // Required references to the library "Database".
        private readonly MessagesDbContext _dbContext;

        public MessageController(MessagesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> MessageTicket([FromBody] PostMessageDto message)
        {
            if (_dbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    // Add new massage for ticket.
                    // Required configuration of connection context with the database in the Startup.cs file.
                    _dbContext.Messages.Add(MessageMapping.PostMessageFromDto(message));
                    await _dbContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status204NoContent);
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
