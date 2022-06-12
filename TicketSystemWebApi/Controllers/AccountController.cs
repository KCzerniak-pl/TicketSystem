using Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TicketSystemWebApi.Helpers.Jwt;
using TicketSystemWebApi.Mapping;
using TicketSystemWebApi.Models;

namespace TicketSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : AuthControllerBase
    {
        // Required references to the library "Database".
        private readonly TicketSystemDbContext _ticketSystemDbContext;

        // JWT - inheritance from class AuthControllerBase.cs.
        // UsersDbContext - required configuration of connection context with the database in the Program.cs file.
        public AccountController(IOptionsMonitor<JwtConfig> optionsMonitor, TicketSystemDbContext ticketSystemDbContext) : base(optionsMonitor)
        {
            _ticketSystemDbContext = ticketSystemDbContext;
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
                    Database.Entities.User user = await _ticketSystemDbContext.Users!.Where(p => p.Email == postLogin.Email).FirstAsync();

                    // AspNetCore Identity.
                    PasswordHasher<string> password = new PasswordHasher<string>();
                    PasswordVerificationResult verificationResult = password.VerifyHashedPassword(postLogin.Email!, user.PasswordHash, postLogin.Password);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        // JWT - create token.
                        var jwt = GenerateJwtToken(user);

                        return StatusCode(StatusCodes.Status200OK, AccountMapping.LoginResponseToDto(true, String.Empty, jwt));
                    }
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, AccountMapping.LoginResponseToDto(false, "Błędny login lub hasło"));
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, AccountMapping.LoginResponseToDto(false, "Nieprawidłowe dane"));
        }

        //GET: api/<ValuesController>/<UserId>
        [HttpGet("{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<GetUsersDto>> GetUserData([FromRoute] Guid userId)
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                try
                {
                    // Retrieving data from database about selected user and remapping to DTO.
                    GetUsersDto user = await _ticketSystemDbContext.Users!.Where(p => p.UserId == userId).Include(p => p.Role).Select(p => AccountMapping.GetUsersToDto(p)).FirstAsync();

                    return StatusCode(StatusCodes.Status200OK, user);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //GET: api/<ValuesController/Technicians
        [HttpGet("Technicians")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<GetUsersDto>>> GetTechniciansData()
        {
            if (_ticketSystemDbContext.Database.CanConnect())
            {
                // Retrieving data from database about all technicians and remapping to DTO.
                IEnumerable<GetUsersDto> users = await _ticketSystemDbContext.Users!.Where(p => p.Role!.Technician == true).Include(p => p.Role).Select(p => AccountMapping.GetUsersToDto(p)).ToArrayAsync();

                if (users.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, users);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
