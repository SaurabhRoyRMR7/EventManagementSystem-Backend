//using EventManagement.UserRepository;
using EventManagement.UserService;
using EventManagementAPI.DTO;
using EventManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3TierEventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        //private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            //_userRepository = userRepository;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserRegistrationDTO registrationDTO)
        {
            try
            {
                var newUser = await _userService.RegisterUserAsync(registrationDTO);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserId }, newUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Handles "Email is already in use."
            }
        }
      
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _userService.AuthenticateUserAsync(loginDTO);

            if (response == null)
            {
                return Unauthorized("Invalid email or password.");
            }

          
          
            return Ok(response);
        }
        [HttpGet("organizerId/{userId}")]
        public async Task<ActionResult<int>> GetOrganizerIdByUserId(int userId)
        {
            var organizerId = await _userService.GetOrganizerIdByUserIdAsync(userId);

            if (organizerId == null)
            {
                return NotFound("Organizer not found for the provided UserId.");
            }

            return Ok(organizerId);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            // Use the repository to get the users
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }
        [HttpPut("user/{action}/{userId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleDTO updateUserRoleDTO)
        {
            try
            {
                var user = await _userService.UpdateUserRoleAsync(userId, updateUserRoleDTO);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("User not found.");
            }
            catch (ArgumentException)
            {
                return BadRequest("Role not found.");
            }
        }

        [HttpDelete("users/{action}/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                bool isDeleted = await _userService.DeleteUserAsync(userId);
                if (isDeleted)
                {
                    return Ok("User deleted successfully.");
                }

                return BadRequest("User deletion failed.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
