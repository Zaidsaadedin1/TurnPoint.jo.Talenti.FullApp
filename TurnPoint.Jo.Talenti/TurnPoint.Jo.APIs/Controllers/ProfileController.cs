using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Common.ProfileDtos;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IUserService userService, ILogger<ProfileController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("GetUserById")]
        public async Task<ActionResult<GenericResponse<GetUserDto>>> GetUserById([FromQuery] int userId)
        {
            _logger.LogInformation("Fetching user with ID {UserId}.", userId);

            var response = await _userService.GetUserByIdAsync(userId);
            if (!response.Success)
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<GenericResponse<IEnumerable<GetUserDto>>>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Fetching users with pagination. Page: {Page}, PageSize: {PageSize}.", page, pageSize);

            var response = await _userService.GetAllUsersAsync();
            if (!response.Success)
            {
                _logger.LogWarning("Failed to fetch users.");
                return BadRequest(response);
            }

            var pagedUsers = response.Data.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new GenericResponse<IEnumerable<GetUserDto>>
            {
                Success = true,
                Message = response.Message,
                Data = pagedUsers
            });
        }

        [AllowAnonymous]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult<GenericResponse<bool>>> UpdateUser([FromQuery] int userId, [FromBody] UpdateUserDto userDto)
        {
            _logger.LogInformation("Updating user with ID {UserId}.", userId);

            var response = await _userService.UpdateUserAsync(userId, userDto);
            if (!response.Success)
            {
                _logger.LogWarning("Failed to update user with ID {UserId}.", userId);
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteUser([FromQuery] int userId)
        {
            _logger.LogInformation("Deleting user with ID {UserId}.", userId);

            var response = await _userService.DeleteUserAsync(userId);
            if (!response.Success)
            {
                _logger.LogWarning("Failed to delete user with ID {UserId}.", userId);
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("CheckIfEmailOrPhoneIsTaken")]
        public async Task<ActionResult<GenericResponse<bool>>> CheckIfEmailOrPhoneIsTaken([FromQuery] string emailOrPhone)
        {
            _logger.LogInformation("Checking if email/phone {EmailOrPhone} is taken.", emailOrPhone);

            var response = await _userService.IsEmailOrPhoneUserTakenAsync(emailOrPhone);
            return Ok(response);
        }
    }
}
