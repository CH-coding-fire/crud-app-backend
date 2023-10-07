using fourthAPI.data;
using fourthAPI.DTOs;
using fourthAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace fourthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CombinedDbContext _context;

        public AuthController(IConfiguration configuration, CombinedDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>Register(UserCreationDTO request)
        {


            // Check if the username already exists
            var existingUser = await _context.Users
                                              .Where(u => u.Username == request.username)
                                              .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Username already exists" });
            }

            User user = new User();

            user.Username = request.username;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.password); ;
            user.TeamId = request.teamId;
            user.Role = request.role;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { Message = "Create user successful", UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {
            // Find user by username
            var user = await _context.Users
                .Where(u => u.Username == request.username)
                .FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            string token = CreateToken(user);
            UserLoginSuccessDTO userLoginSuccessDTO = new UserLoginSuccessDTO
            {
                message = "Login successful",
                username = user.Username,
                role = user.Role,
                token = token,
                teamId = user.TeamId
            };

            // Authentication successful: return the user object or a token
            return Ok(userLoginSuccessDTO);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("userId", user.Id.ToString()),
            new Claim("teamId", user.TeamId.ToString()),
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }




    }
}
