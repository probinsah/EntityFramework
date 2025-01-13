using DbOperationsWithEFCoreApp.Data;
using DbOperationsWithEFCoreApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly JWTTokenGenerator _jwtTokenGenerator;

        public UsersController(AppDbContext appDbContext, JWTTokenGenerator jwtTokenGenerator)
        {
            _appDbContext = appDbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _appDbContext.Users.Include(x => x.Profile).ToListAsync();

            return Ok(users);
        }

        //1. Create a New User With Profile
        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null) return BadRequest("User data is required.");

            User userExist = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);
            if(userExist != null)
            {
                string message = userExist.Email == user.Email
                ? "An account with this email already exists."
                : "This username is already taken. Please choose a different one.";
                return BadRequest(message);
            }
            user.PasswordHash = PasswordUtility.HashPassword(user.PasswordHash);

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new {id  = user.Id }, user);
        }
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Username) && string.IsNullOrEmpty(login.EmailId))
                {
                    return BadRequest("Please provide an email or username.");
                }

                if (string.IsNullOrEmpty(login.PasswordHash))
                {
                    return BadRequest("Password is required.");
                }

                var user = await _appDbContext.Users.FirstOrDefaultAsync(x => 
                ((!string.IsNullOrEmpty(login.Username) && x.Username == login.Username) || 
                (!string.IsNullOrEmpty(login.EmailId) && x.Email == login.EmailId)) && x.IsActive == true);

                if(user == null)
                {
                    return Unauthorized("Invalid Username/Email or Password.");
                }
                bool isPasswordValid = PasswordUtility.VerifyPassword(login.PasswordHash, user.PasswordHash);
                
                if (!isPasswordValid) 
                {
                    return Unauthorized("Invalid Username/Email or Password.");
                }
                var token = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Username);
                
                return Ok(new
                {
                    Token  = token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An Error Occur While Login Process. " + ex);
            }
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPassword forgetPassword)
        {
            if(string.IsNullOrEmpty(forgetPassword.Email) || string.IsNullOrEmpty(forgetPassword.dateOfbirth.ToString()))
            {
                return BadRequest("Email and Date of Birth is required.");
            }
            var user = await _appDbContext.Users.Include(x => x.Profile).FirstOrDefaultAsync(x => 
            x.Email ==  forgetPassword.Email && x.Profile.DateOfBirth == forgetPassword.dateOfbirth);

            if(user == null)
            {
                return BadRequest("The email id or date of birth is incorrect.");
            }
            return Ok(new { Message = "Varification Successfull, you can reset the password." });
        }
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            if(string.IsNullOrEmpty(resetPassword.Email) || string.IsNullOrEmpty(resetPassword.Password))
            {
                return BadRequest("Email Id and New Password is Required.");
            }

            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == resetPassword.Email);

            if(user == null)
            {
                return BadRequest("Invalid Email Id");
            }
            user.PasswordHash = PasswordUtility.HashPassword(resetPassword.Password);
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();

            return Ok("Password reset successfully.");
        }
        //2. Get User By Id (with UserProfile)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var user = await _appDbContext.Users.Include(x => x.Profile).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound($"User with Id {id} does not exist.");
            return Ok(user);
        }

        //Update User data
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                var user = await _appDbContext.Users.FindAsync(id);
                user.Email = updatedUser.Email;

                _appDbContext.Update(user);
                await _appDbContext.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Error Occur while updating user data-"+ex);
            }
        }
        [HttpPut("{userId}/profile")]
        public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfile updatedProfile)
        {
            var profile = await _appDbContext.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId);

            if (profile == null) return NotFound($"Profile for User ID {userId} not found.");

            profile.Address = updatedProfile.Address;
            profile.DateOfBirth = updatedProfile.DateOfBirth;
            profile.ProfilePictureUrl = updatedProfile.ProfilePictureUrl;

            _appDbContext.UserProfiles.Update(profile);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _appDbContext.Users.Include(u => u.Profile).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return NotFound($"User ID {id} does not exist.");

            _appDbContext.Remove(user);
            await _appDbContext.SaveChangesAsync();

            return Ok("Successfully Deleted");
        }
    }
}
