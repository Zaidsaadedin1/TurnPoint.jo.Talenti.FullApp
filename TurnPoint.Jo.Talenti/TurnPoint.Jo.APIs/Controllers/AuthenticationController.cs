using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Interfaceses;

namespace TurnPoint.Jo.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registerUserDto == null)
            {
                return BadRequest("Invalid registration details.");
            }

            var registered = await _authService.RegisterUserAsync(registerUserDto);
            if (!registered)
            {
                return BadRequest("Failed to register new user.");
            }
            return Ok("User Registered Successfully");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (loginDto == null)
            {
                return BadRequest("Invalid login details.");
            }

            var token = await _authService.LoginUserAsync(loginDto);
            if (token == null)
            {
                return BadRequest("Failed to login.");
            }
            return Ok(new { Token = token, message = "User logged in successfully." });
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (resetPasswordDto == null || string.IsNullOrWhiteSpace(resetPasswordDto.EmailOrPhone))
            {
                return BadRequest("Invalid details.");
            }

            var result = await _authService.UserPasswordResetAsync(resetPasswordDto);
            if (!result)
            {
                return BadRequest("Failed to initiate password reset.");
            }

            return Ok("OTP sent successfully to your email or phone.");
        }
    }
}
