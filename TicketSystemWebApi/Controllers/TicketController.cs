using Database;
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
    public class TicketController : ControllerBase
    {
        private readonly TicketsDbContext _ticketsDbContext;
        private readonly UsersDbContext _usersDbContext;
        private readonly StatusesDbContext _statusesDbContext;

        // Required configuration of connection context with the database in the Program.cs file and references to the library "Database".
        public TicketController(TicketsDbContext tickeetsDbContext, UsersDbContext usersDbContext, StatusesDbContext statusesDbContext)
        {
            _ticketsDbContext = tickeetsDbContext;
            _usersDbContext = usersDbContext;
            _statusesDbContext = statusesDbContext;
        }

        //GET: api/<ValuesController>?ticketID=<ticketID>&userID=<userID>
        [HttpGet]
        public async Task<ActionResult<GetTicketDto>> GetTicketByID([FromQuery, BindRequired] Guid ticketID, [FromQuery, BindRequired] Guid userID)
        {
            if (_ticketsDbContext.Database.CanConnect())
            {
                try
                {
                    // Retrieving data from database about selected ticket and remapping to DTO.
                    GetTicketDto ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == ticketID).Include(p => p.Owner).Include(p => p.Category).Include(p => p.Status).Include(p => p.Messages).ThenInclude(p => p.Owner).Select(p => TicketMapping.GetTicketToDto(p)).FirstAsync();

                    // Retrieving data from database about selected user.
                    Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == userID).Include(p => p.Role).FirstAsync();

                    // Verification that user can get ticket (must be its author or have permission to view all tickets).
                    if (ticket.UserID == userID || user.Role.ShowAll == true)
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
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about status for a new ticket (first element).
                        Guid statusForNewTicket = await _statusesDbContext.Statuses.Select(p => p.StatusID).FirstAsync();

                        // Add new ticket.
                        Database.Entities.Ticket ticket = TicketMapping.PostTicketFromDto(postTicket, statusForNewTicket);
                        _ = _ticketsDbContext.Tickets.Add(ticket);
                        _ = await _ticketsDbContext.SaveChangesAsync();

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
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == putTicket.TicketID).Include(p => p.Owner).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == putTicket.UserID).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerID == putTicket.UserID || user.Role.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketStatusFromDto(ticket, putTicket);
                            _ = await _ticketsDbContext.SaveChangesAsync();

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
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == putTicket.TicketID).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == putTicket.UserID).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerID == putTicket.UserID || user.Role.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketTitleFromDto(ticket, putTicket);
                            _ = await _ticketsDbContext.SaveChangesAsync();

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
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == putTicket.TicketID).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == putTicket.UserID).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerID == putTicket.UserID || user.Role.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketCategoryFromDto(ticket, putTicket);
                            _ = await _ticketsDbContext.SaveChangesAsync();

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
            if (_ticketsDbContext.Database.CanConnect())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Retrieving data from database about selected ticket.
                        Database.Entities.Ticket ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == putTicket.TicketID).FirstAsync();

                        // Retrieving data from database about selected user.
                        Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == putTicket.UserID).Include(p => p.Role).FirstAsync();

                        // Verification that user can update ticket (must be its author or have permission to view all tickets).
                        if (ticket.OwnerID == putTicket.UserID || user.Role.ShowAll == true)
                        {
                            // Update data in database about selected ticket.
                            _ = TicketMapping.PutTicketTechnicianFromDto(ticket, putTicket);
                            _ = await _ticketsDbContext.SaveChangesAsync();

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
            if (_ticketsDbContext.Database.CanConnect())
            {
                try
                {
                    // Retrieving data from database about selected ticket.
                    Database.Entities.Ticket ticket = await _ticketsDbContext.Tickets.Where(p => p.TicketID == deleteTicket.TicketID).Include(p => p.Messages).FirstAsync();

                    // Retrieving data from database about selected user.
                    Database.Entities.User user = await _usersDbContext.Users.Where(p => p.UserID == deleteTicket.UserID).Include(p => p.Role).FirstAsync();

                    // Verification that user can delete ticket (must be its author or have permission to view all tickets).
                    if (ticket.OwnerID == deleteTicket.UserID || user.Role.ShowAll == true)
                    {
                        // Remove data id database about selected ticket.
                        _ = _ticketsDbContext.Tickets.Remove(ticket);
                        _ = await _ticketsDbContext.SaveChangesAsync();

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
