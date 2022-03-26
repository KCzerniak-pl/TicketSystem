using Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystemWebApi.Mapping;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        // Required references to the library "Database".
        private readonly UsersDbContext _dbContext;

        public AccountController(UsersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //GET: api//<ValuesController>
        [HttpPost]
        public async Task<ActionResult<LoginResponseDto>> PostLoginRequest([FromBody] LoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                // Get user by e-mail address.
                var result = await _dbContext.Users.SingleOrDefaultAsync(p => p.Email == user.Email);

                if (result != null)
                {
                    // AspNetCore Identity.
                    PasswordHasher<string> password = new PasswordHasher<string>();
                    var verificationResult = password.VerifyHashedPassword(user.Email, result.PasswordHash, user.Password);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, Mapping.AccountMapping.LoginResponseToDto(result, true, null));
                    }
                }

                return StatusCode(StatusCodes.Status400BadRequest, Mapping.AccountMapping.LoginResponseToDto(null, false, "Błędny login lub hasło"));
            }

            return StatusCode(StatusCodes.Status400BadRequest, Mapping.AccountMapping.LoginResponseToDto(null, false, "Nieprawidłowe dane"));
        }

        //GET: api/<ValuesController>/<UserID>
        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetUserData([FromRoute] Guid userID)
        {
            if (_dbContext.Database.CanConnect())
            {
                // Retrieving data from database about all ticket of the selected user and remapping to DTO.
                // Required configuration of connection context with the database in the Startup.cs file.
                var result = await _dbContext.Users.Where(p => p.UserID == userID).Include(p => p.Role).Select(p => AccountMapping.GetUsersToDto(p)).ToListAsync();

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
