using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Common.Shared;
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
        public async Task<ActionResult<GenericResponse<bool>>> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid registration details."
                });
            }

            if (registerUserDto == null)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid registration details."
                });
            }

            var registered = await _authService.RegisterUserAsync(registerUserDto);
            if (!registered)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Failed to register new user."
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = true
            });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<GenericResponse<string>>> Login([FromBody] LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "Invalid login details."
                });
            }

            if (loginDto == null)
            {
                return BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "Invalid login details."
                });
            }

            var token = await _authService.LoginUserAsync(loginDto);
            if (token == null)
            {
                return BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "Failed to login."
                });
            }

            return Ok(new GenericResponse<string>
            {
                Success = true,
                Message = "User logged in successfully.",
                Data = token
            });
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<GenericResponse<bool>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid details."
                });
            }

            var result = await _authService.UserPasswordResetAsync(resetPasswordDto);
            if (!result)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Failed to initiate password reset."
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = "OTP sent successfully to your email or phone.",
                Data = true
            });
        }
    }
}
