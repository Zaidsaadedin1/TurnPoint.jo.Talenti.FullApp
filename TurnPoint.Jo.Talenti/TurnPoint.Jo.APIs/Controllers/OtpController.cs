using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Enums;
using TurnPoint.Jo.APIs.Common.VerificationsDtos;

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
        [HttpPost("/SendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] string emailOrPhone)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(emailOrPhone))
            {
                return BadRequest("Invalid email or phone number.");
            }

            var result = await _otpService.SendOtpAsync(emailOrPhone);
            if (!result)
            {
                return BadRequest("Failed to send OTP.");
            }

            return Ok("OTP sent successfully.");
        }

        [AllowAnonymous]
        [HttpPost("/VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (verifyOtpDto == null || string.IsNullOrWhiteSpace(verifyOtpDto.Otp))
            {
                return BadRequest("Invalid OTP details.");
            }

            var verificationResult = await _otpService.VerifyUserOtpAsync(verifyOtpDto);
            switch (verificationResult)
            {
                case VerificationResult.UserNotFound:
                    return BadRequest("User not found.");
                case VerificationResult.OtpNotFound:
                    return BadRequest("OTP not found.");
                case VerificationResult.OtpExpired:
                    return BadRequest("OTP expired.");
                case VerificationResult.InvalidOtp:
                    return BadRequest("Invalid OTP.");
                case VerificationResult.Success:
                    return Ok("OTP verified successfully.");
                default:
                    return BadRequest("An error occurred while verifying OTP.");
            }
        }
    }
}
