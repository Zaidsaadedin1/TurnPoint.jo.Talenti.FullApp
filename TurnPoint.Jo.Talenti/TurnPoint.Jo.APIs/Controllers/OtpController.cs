using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Enums;
using TurnPoint.Jo.APIs.Common.VerificationsDtos;
using TurnPoint.Jo.APIs.Common.Shared;
using TurnPoint.Jo.APIs.Entities;
using Microsoft.AspNetCore.Identity;

namespace TurnPoint.Jo.APIs.Controllers
{
    [Route("api/otp")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly ILogger<OtpController> _logger;

        public OtpController(IOtpService otpService, ILogger<OtpController> logger)
        {
            _otpService = otpService;
            _logger = logger;
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

            _logger.LogInformation("Processing SendOtp request for {EmailOrPhone}", emailOrPhone);

            var result = await _otpService.SendOtpAsync(emailOrPhone);
            if (!result.Success)
            {
                _logger.LogWarning("Failed to send OTP for {EmailOrPhone}", emailOrPhone);
                return BadRequest(result);
            }

            _logger.LogInformation("OTP sent successfully to {EmailOrPhone}", emailOrPhone);
            return Ok(result);
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

            _logger.LogInformation("Processing VerifyOtp request for {EmailOrPhone}", verifyOtpDto.EmailOrPhone);

            var verificationResult = await _otpService.VerifyUserOtpAsync(verifyOtpDto);
            var response = verificationResult switch
            {
                VerificationResult.UserNotFound => new GenericResponse<string>
                {
                    Success = false,
                    Message = "User not found.",
                    Data = null
                },
                VerificationResult.OtpNotFound => new GenericResponse<string>
                {
                    Success = false,
                    Message = "OTP not found.",
                    Data = null
                },
                VerificationResult.OtpExpired => new GenericResponse<string>
                {
                    Success = false,
                    Message = "OTP expired.",
                    Data = null
                },
                VerificationResult.InvalidOtp => new GenericResponse<string>
                {
                    Success = false,
                    Message = "Invalid OTP.",
                    Data = null
                },
                VerificationResult.Success => new GenericResponse<string>
                {
                    Success = true,
                    Message = "OTP verified successfully.",
                    Data = "Verified"
                },
                _ => new GenericResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while verifying OTP.",
                    Data = null
                },
            };

            if (!response.Success)
            {
                _logger.LogWarning("Failed to verify OTP for {EmailOrPhone}: {Message}", verifyOtpDto.EmailOrPhone, response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("OTP verified successfully for {EmailOrPhone}", verifyOtpDto.EmailOrPhone);
            return Ok(response);
        }
    }

}
