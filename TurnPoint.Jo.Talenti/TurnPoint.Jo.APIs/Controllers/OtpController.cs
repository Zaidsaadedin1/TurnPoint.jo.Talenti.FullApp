using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Enums;
using TurnPoint.Jo.APIs.Common.VerificationsDtos;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [AllowAnonymous]
        [HttpPost("SendOtp")]
        public async Task<ActionResult<GenericResponse<bool>>> SendOtp([FromBody] string emailOrPhone)
        {
            if (string.IsNullOrWhiteSpace(emailOrPhone))
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid email or phone number.",
                    Data = false
                });
            }

            var result = await _otpService.SendOtpAsync(emailOrPhone);
            if (!result)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Failed to send OTP.",
                    Data = false
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = "OTP sent successfully.",
                Data = true
            });
        }

        [AllowAnonymous]
        [HttpPost("VerifyOtp")]
        public async Task<ActionResult<GenericResponse<string>>> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            if (verifyOtpDto == null || string.IsNullOrWhiteSpace(verifyOtpDto.Otp))
            {
                return BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "Invalid OTP details.",
                    Data = null
                });
            }

            var verificationResult = await _otpService.VerifyUserOtpAsync(verifyOtpDto);
            return verificationResult switch
            {
                VerificationResult.UserNotFound => BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                }),
                VerificationResult.OtpNotFound => BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "OTP not found.",
                    Data = null
                }),
                VerificationResult.OtpExpired => BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "OTP expired.",
                    Data = null
                }),
                VerificationResult.InvalidOtp => BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "Invalid OTP.",
                    Data = null
                }),
                VerificationResult.Success => Ok(new GenericResponse<string>
                {
                    Success = true,
                    Message = "OTP verified successfully.",
                    Data = "Verified"
                }),
                _ => BadRequest(new GenericResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while verifying OTP.",
                    Data = null
                }),
            };
        }
    }
}
