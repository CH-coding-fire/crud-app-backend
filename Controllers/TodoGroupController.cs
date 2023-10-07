using fourthAPI.data;
using fourthAPI.DTOs;
using fourthAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fourthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class TodoGroupController:ControllerBase
    {
        private readonly CombinedDbContext _context;
        public TodoGroupController(CombinedDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoGroup>>> GetTodoGroups()
        {
            // Retrieve the UserId and Role claim value from the JWT
            var userIdClaim = User.FindFirst("userId")?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userIdClaim == null || roleClaim == null)
            {
                return Unauthorized();
            }

            if (roleClaim == "Admin")
            {
                var allTodoGroups = await _context.TodoGroups.ToListAsync();
                return Ok(allTodoGroups);
            }
            else if (roleClaim == "Member")
            {
                int userId;
                if (!int.TryParse(userIdClaim, out userId))
                {
                    return BadRequest("Invalid user ID");
                }

                User user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound();
                }

                var todoGroups = await _context.TodoGroups
                                   .Where(tg => tg.TeamId == user.TeamId)
                                   .ToListAsync();

                return Ok(todoGroups);
            }
            else
            {
                return Unauthorized();
            }
        }


        [HttpGet("{todoGroupId}")]
        public async Task<ActionResult<TodoGroup>> GetTodoGroupById(int todoGroupId)
        {
            // Retrieve the UserId and Role claim value from the JWT
            var userIdClaim = User.FindFirst("userId")?.Value;
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            var teamIdClaim = User.FindFirst("teamId")?.Value;


            if (userIdClaim == null || roleClaim == null )
            {
                return Unauthorized();
            }


            if (roleClaim == "Admin")
            {
                TodoGroup todoGroup = await _context.TodoGroups.FindAsync(todoGroupId);
                todoGroup.TodoItems = await _context.TodoItems
                              .Where(ti => ti.TodoGroupId == todoGroupId)
                              .ToArrayAsync();
                return Ok(todoGroup);
            }
            else if (roleClaim == "Member")
            {
                if (!int.TryParse(userIdClaim, out int userId) || !int.TryParse(teamIdClaim, out int teamId))
                {
                    return BadRequest("Invalid user or team ID");
                }

                TodoGroup todoGroup = await _context.TodoGroups.FindAsync(todoGroupId);
                if(todoGroup.TeamId == teamId)
                {
                    return Ok(todoGroup);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(TodoGroupCreationDTO request)
        {
            TodoGroup todoGroup = new TodoGroup();
            todoGroup.TeamId = request.teamId ;
            todoGroup.Created = DateTime.Now.ToUniversalTime();
            todoGroup.Name = request.name;

            await _context.TodoGroups.AddAsync(todoGroup);
            await _context.SaveChangesAsync();
            return StatusCode(201, new { Message = "Create todo group successful", TodoGroup = todoGroup});
        }
    }
}
