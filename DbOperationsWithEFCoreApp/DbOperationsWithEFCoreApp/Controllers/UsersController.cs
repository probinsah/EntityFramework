using DbOperationsWithEFCoreApp.Data;
using DbOperationsWithEFCoreApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UsersController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //1. Create a New User With Profile
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null) return BadRequest("User data is required.");

            user.PasswordHash = PasswordUtility.HashPassword(user.PasswordHash);

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new {id  = user.Id }, user);
        }

        //2. Get User By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _appDbContext.Users.Include(x => x.Profile).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound($"User with Id {id} does not exist.");
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _appDbContext.Users.Include(x => x.Profile).ToListAsync();

            return Ok(users);
        }
    }
}
