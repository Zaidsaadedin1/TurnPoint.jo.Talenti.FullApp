using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Enums;
using Microsoft.AspNetCore.Identity;
using TurnPoint.Jo.APIs.Entities;
using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Common.VerificationsDtos;
using TurnPoint.Jo.APIs.Common.AuthDtos;

namespace TurnPoint.Jo.APIs.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly ILogger<OtpService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public OtpService(IEmailService emailService, ISmsService smsService, ILogger<OtpService> logger, UserManager<User> userManager, IConfiguration configuration)
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

        public async Task<bool> SendOtpAsync(string emailOrPhone)
        {
            var otp = GenerateOtp();
            var otpExpiresAt = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("OtpSettings:ExpiryTimeInMinutes"));

            var user = await _userManager.Users
                   .FirstOrDefaultAsync(u => u.PhoneNumber == emailOrPhone || u.Email == emailOrPhone);

            if (user == null)
            {
                _logger.LogError("User with email/phone {EmailOrPhone} not found.", emailOrPhone);
                return false;
            }

            user.Otp = otp;
            user.OtpExpiresAt = otpExpiresAt;
            await _userManager.UpdateAsync(user);

            if (emailOrPhone.Contains('@'))
            {
                var emailSubject = "Welcome to Talenti!";
                var emailBody = $"Dear User,\n\nThank you for registering. Your OTP is: {otp}\n\nBest regards,\nTalenti Team";
                var emailStatus = await _emailService.SendEmailToUserAsync(emailOrPhone, emailSubject, emailBody);

                if (emailStatus != EmailResponseStatus.Success)
                {
                    _logger.LogError("Failed to send registration email to {Email}.", emailOrPhone);
                    return false;
                }
            }
            else
            {   
                var smsStatus = await _smsService.SendSmsToUserAsync(emailOrPhone, $"Your OTP is: ",otp);

                if (smsStatus != SmsResponseStatus.Success)
                {
                    _logger.LogError("Failed to send OTP to {PhoneNumber}.", emailOrPhone);
                    return false;
                }
            }

            return true;
        }

        public async Task<VerificationResult> VerifyUserOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == verifyOtpDto.EmailOrPhone || u.Email == verifyOtpDto.EmailOrPhone); if (user == null)
                return VerificationResult.UserNotFound;

            if (string.IsNullOrEmpty(user.Otp))
                return VerificationResult.OtpNotFound;

            if (user.OtpExpiresAt < DateTime.UtcNow)
                return VerificationResult.OtpExpired;

            if (user.Otp != verifyOtpDto.Otp)
                return VerificationResult.InvalidOtp;

            user.Otp = null;
            user.OtpExpiresAt = null;

            if (verifyOtpDto.EmailOrPhone.Equals("phone", StringComparison.OrdinalIgnoreCase))
            {
                user.PhoneNumberConfirmed = true;
            }
            else if (verifyOtpDto.EmailOrPhone.Equals("email", StringComparison.OrdinalIgnoreCase))
            {
                user.EmailConfirmed = true;
            }

            await _userManager.UpdateAsync(user);

            return VerificationResult.Success;
        }
    }
}
