using Database;
using EmailService;
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
    public class TicketController : ControllerBase
    {
        private readonly TicketSystemDbContext _ticketSystemDbContext;
        private readonly IEmailSender _emailSender;

        // Required configuration of connection context with the database in the Program.cs file and references to the library "Database".
        // Dependency injection - inverse of control (for email sending). Required configuration in the Program.cs.
        public TicketController(TicketSystemDbContext ticketSystemDbContext, IEmailSender emailSender)
        {
            _ticketSystemDbContext = ticketSystemDbContext;
            _emailSender = emailSender;
        }

        //GET: api/<ValuesController>?ticketId=<ticketId>&userId=<userId>
        [HttpGet]
        public async Task<ActionResult<GetTicketDto>> GetTicketByID([FromQuery, BindRequired] Guid ticketId, [FromQuery, BindRequired] Guid userId)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                try
                {
                    // Retrieving data from database about selected ticket and remapping to DTO.
                    GetTicketDto ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == ticketId).Include(p => p.Owner).Include(p => p.Category).Include(p => p.Status).Include(p => p.Messages)!.ThenInclude(p => p.Owner).Select(p => TicketMapping.GetTicketToDto(p)).FirstAsync();

                    // Retrieving data from database about selected user.
                    Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == userId).Include(p => p.Role).FirstAsync();

                    // Verification that user can get ticket (must be its author or have permission to view all tickets).
                    if (ticket.UserId == userId || user.Role!.ShowAll == true)
                    {
                        return StatusCode(StatusCodes.Status200OK, ticket);
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromBody] PostTicketDto postTicket)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about status for a new ticket (first element).
                        Guid statusForNewTicket = await _ticketSystemDbContext.Statuses!.Select(p => p.StatusId).FirstAsync();

                        // Add new ticket.
                        Database.Entities.Ticket ticket = TicketMapping.PostTicketFromDto(postTicket, statusForNewTicket);
                        _ = _ticketSystemDbContext.Tickets!.Add(ticket);
                        _ = await _ticketSystemDbContext.SaveChangesAsync();

                        // Retrieving data from database about user who added new ticket.
                        Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == postTicket.UserId).FirstAsync();
                        // Retrieving data from database about selected ticket - after added.
                        ticket = await _ticketSystemDbContext.Tickets.Where(p => p.TicketId == ticket.TicketId).Include(p => p.Status).Include(p => p.Owner).FirstAsync();
                        // Send e-mail.
                        Helpers.SendEmail.Send(_emailSender, 1, ticket);

                        return StatusCode(StatusCodes.Status204NoContent);
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

        // PUT api/<ValuesController>/StatusUpdate
        [HttpPut("StatusUpdate")]
        public async Task<IActionResult> PutTicketStatus([FromBody] PutTicketStatusDto putTicket)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == putTicket.TicketId).Include(p => p.Owner).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == putTicket.UserId).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerId == putTicket.UserId || user.Role!.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketStatusFromDto(ticket, putTicket);
                            _ = await _ticketSystemDbContext.SaveChangesAsync();

                            // Retrieving data from database about selected ticket - after update.
                            ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == putTicket.TicketId).Include(p => p.Status).Include(p => p.Owner).FirstAsync();
                            // Send e-mail.
                            Helpers.SendEmail.Send(_emailSender, 2, ticket, user);

                            return StatusCode(StatusCodes.Status204NoContent);
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status403Forbidden);
                        }
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PUT api/<ValuesController>/TitleUpdate
        [HttpPut("TitleUpdate")]
        public async Task<IActionResult> PutTicketTitle([FromBody] PutTicketTitleDto putTicket)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == putTicket.TicketId).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == putTicket.UserId).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerId == putTicket.UserId || user.Role!.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketTitleFromDto(ticket, putTicket);
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
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PUT api/<ValuesController>/CategoryUpdate
        [HttpPut("CategoryUpdate")]
        public async Task<IActionResult> PutTicketCategory([FromBody] PutTicketCategoryDto putTicket)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == putTicket.TicketId).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == putTicket.UserId).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerId == putTicket.UserId || user.Role!.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketCategoryFromDto(ticket, putTicket);
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
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // PUT api/<ValuesController>/TechnicianUpdate
        [HttpPut("TechnicianUpdate")]
        public async Task<IActionResult> TechnicianUpdate([FromBody] PutTicketTechnicianDto putTicket)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == putTicket.TicketId).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == putTicket.UserId).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerId == putTicket.UserId || user.Role!.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketTechnicianFromDto(ticket, putTicket);
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
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // DELETE api/<ValuesController>
        [HttpDelete]
        public async Task<IActionResult> DeleteTicket([FromBody] DeleteTicketDto deleteTicket)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                try
                {
                    // Retrieving data from database about selected ticket.
                    Database.Entities.Ticket ticket = await _ticketSystemDbContext.Tickets!.Where(p => p.TicketId == deleteTicket.TicketId).Include(p => p.Messages).FirstAsync();

                    // Retrieving data from database about selected user.
                    Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == deleteTicket.UserId).Include(p => p.Role).FirstAsync();

                    // Verification that user can delete ticket (must be its author or have permission to view all tickets).
                    if (ticket.OwnerId == deleteTicket.UserId || user.Role!.ShowAll == true)
                    {
                        // Remove data id database about selected ticket.
                        _ = _ticketSystemDbContext.Tickets!.Remove(ticket);
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
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
