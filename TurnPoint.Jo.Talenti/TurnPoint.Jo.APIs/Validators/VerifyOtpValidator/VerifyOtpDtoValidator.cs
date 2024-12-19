using FluentValidation;
using TurnPoint.Jo.APIs.Common.VerificationsDtos;

namespace TurnPoint.Jo.APIs.Validators
{
    public class VerifyOtpDtoValidator : AbstractValidator<VerifyOtpDto>
    {
        public VerifyOtpDtoValidator()
        {
            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .Matches(@"^\d{6}$").WithMessage("OTP must be exactly 6 digits.");

            RuleFor(x => x.EmailOrPhone)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email format is incorrect.");

           

        }
    }
}
