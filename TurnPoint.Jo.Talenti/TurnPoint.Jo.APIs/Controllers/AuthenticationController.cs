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
        public async Task<ActionResult<GenericResponse<RegisterUserDto>>> Register([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new GenericResponse<RegisterUserDto>
                {
                    Success = false,
                    Message = "Invalid registration details. Something is missing.",
                    Data = null
                });
            }

            var response = await _authService.RegisterUserAsync(registerUserDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
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

            var response = await _authService.LoginUserAsync(loginDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
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

            var response = await _authService.UserPasswordResetAsync(resetPasswordDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
