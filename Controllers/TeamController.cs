using fourthAPI.data;
using fourthAPI.DTOs;
using fourthAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace fourthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly CombinedDbContext _context;
        public TeamController(CombinedDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Team>> Get()
        {
            return (IEnumerable<Team>)await _context.Teams.ToListAsync();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(TeamRequestDTO request)
        {

            Team team = new Team();
            team.TeamName = request.TeamName;
            team.Created = DateTime.Now.ToUniversalTime();

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return team;
        }
    }
    
}
