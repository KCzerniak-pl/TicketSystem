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
        private readonly UsersDbContext _usersDbContext;

        // Required configuration of connection context with the database in the Program.cs file.
        public AccountController(UsersDbContext usersDbContext)
        {
            _usersDbContext = usersDbContext;
        }

        //GET: api//<ValuesController>
        [HttpPost]
        public async Task<ActionResult<LoginResponseDto>> PostLoginRequest([FromBody] LoginRequestDto postLogin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Get user by e-mail address.
                    Database.Entities.User user = await _usersDbContext.Users.Where(p => p.Email == postLogin.Email).FirstAsync();

                    // AspNetCore Identity.
                    PasswordHasher<string> password = new PasswordHasher<string>();
                    PasswordVerificationResult verificationResult = password.VerifyHashedPassword(postLogin.Email, user.PasswordHash, postLogin.Password);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        return StatusCode(StatusCodes.Status200OK, AccountMapping.LoginResponseToDto(true, String.Empty, user));
                    }
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, AccountMapping.LoginResponseToDto(false, "Błędny login lub hasło", null));
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, AccountMapping.LoginResponseToDto(false, "Nieprawidłowe dane", null));
        }

        //GET: api/<ValuesController>/<UserID>
        [HttpGet("{userID}")]
        public async Task<ActionResult<GetUsersDto>> GetUserData([FromRoute] Guid userID)
        {
            if (_usersDbContext.Database.CanConnect())
            {
                try
                {
                    // Retrieving data from database about all ticket of the selected user and remapping to DTO.
                    GetUsersDto user = await _usersDbContext.Users.Where(p => p.UserID == userID).Include(p => p.Role).Select(p => AccountMapping.GetUsersToDto(p)).FirstAsync();

                    return StatusCode(StatusCodes.Status200OK, user);
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
