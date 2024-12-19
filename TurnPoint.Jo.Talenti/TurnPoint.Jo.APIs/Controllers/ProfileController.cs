using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Common.ProfileDtos;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Authorization;

namespace TurnPoint.Jo.APIs.Controllers
{
    [ApiController]
    [Route("api/Profiles")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IUserService userService, ILogger<ProfileController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] int userId)
        {
            _logger.LogInformation("Fetching user with ID {UserId}.", userId);

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Fetching users with pagination. Page: {Page}, PageSize: {PageSize}.", page, pageSize);

            var users = await _userService.GetAllUsersAsync();
            var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(pagedUsers);
        }

        [AllowAnonymous]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromQuery] int userId, [FromBody] UpdateUserDto userDto)
        {
            _logger.LogInformation("Updating user with ID {UserId}.", userId);

            var success = await _userService.UpdateUserAsync(userId, userDto);
            if (!success)
            {
                _logger.LogWarning("Failed to update user with ID {UserId}.", userId);
                return BadRequest(new { Message = "Failed to update user" });
            }

            return Ok(new { Message = "User updated successfully" });
        }

        [AllowAnonymous]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] int userId)
        {
            _logger.LogInformation("Deleting user with ID {UserId}.", userId);

            var success = await _userService.DeleteUserAsync(userId);
            if (!success)
            {
                _logger.LogWarning("Failed to delete user with ID {UserId}.", userId);
                return NotFound(new { Message = "User not found" });
            }

            return Ok(new { Message = "User deleted successfully" });
        }

        [AllowAnonymous]
        [HttpGet("CheckIfEmailOrPhoneIsTaken")]
        public async Task<IActionResult> CheckIfEmailOrPhoneIsTaken([FromQuery] string emailOrPhone)
        {
            _logger.LogInformation("Checking if email/phone {EmailOrPhone} is taken.", emailOrPhone);

            var isTaken = await _userService.IsEmailOrPhoneUserTakenAsync(emailOrPhone);
            return Ok(new { IsTaken = isTaken });
        }
    }
}
