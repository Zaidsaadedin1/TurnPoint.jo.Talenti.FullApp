using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Enums;
using TurnPoint.Jo.APIs.Common.VerificationsDtos;
using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly ILogger<OtpService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public OtpService(
            IEmailService emailService,
            ISmsService smsService,
            ILogger<OtpService> logger,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _emailService = emailService;
            _smsService = smsService;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<GenericResponse<bool>> SendOtpAsync(string emailOrPhone)
        {
            var otp = GenerateOtp();
            var expiry = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("OtpSettings:ExpiryTimeInMinutes"));

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == emailOrPhone || u.Email == emailOrPhone);
            if (user == null)
            {
                _logger.LogError("User with email/phone {EmailOrPhone} not found.", emailOrPhone);
                return new GenericResponse<bool> { Success = false, Message = "User not found." };
            }

            user.Otp = otp;
            user.OtpExpiresAt = expiry;
            await _userManager.UpdateAsync(user);

            if (emailOrPhone.Contains("@"))
            {
                var emailStatus = await _emailService.SendEmailToUserAsync(
                    emailOrPhone,
                    "Your OTP",
                    $"Your OTP is {otp}");

                if (emailStatus != EmailResponseStatus.Success)
                {
                    _logger.LogError("Failed to send OTP email to {Email}.", emailOrPhone);
                    return new GenericResponse<bool> { Success = false, Message = "Failed to send OTP email." };
                }
            }
            else
            {
                var smsStatus = await _smsService.SendSmsToUserAsync(emailOrPhone, "Your OTP", otp);

                if (smsStatus != SmsResponseStatus.Success)
                {
                    _logger.LogError("Failed to send OTP SMS to {Phone}.", emailOrPhone);
                    return new GenericResponse<bool> { Success = false, Message = "Failed to send OTP SMS." };
                }
            }

            return new GenericResponse<bool> { Success = true, Message = "OTP sent successfully." };
        }

        public async Task<VerificationResult> VerifyUserOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == verifyOtpDto.EmailOrPhone || u.Email == verifyOtpDto.EmailOrPhone);

            if (user == null) return VerificationResult.UserNotFound;

            if (string.IsNullOrEmpty(user.Otp)) return VerificationResult.OtpNotFound;

            if (user.OtpExpiresAt < DateTime.UtcNow) return VerificationResult.OtpExpired;

            if (user.Otp != verifyOtpDto.Otp) return VerificationResult.InvalidOtp;

            user.Otp = null;
            user.OtpExpiresAt = null;

            if (verifyOtpDto.EmailOrPhone.Contains("@"))
                user.EmailConfirmed = true;
            else
                user.PhoneNumberConfirmed = true;

            await _userManager.UpdateAsync(user);
            return VerificationResult.Success;
        }
    }
}

