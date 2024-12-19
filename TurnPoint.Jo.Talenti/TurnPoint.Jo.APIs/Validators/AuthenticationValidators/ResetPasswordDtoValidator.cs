using FluentValidation;
using TurnPoint.Jo.APIs.Common.AuthDtos;

namespace TurnPoint.Jo.APIs.Validators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.EmailOrPhone)
              .NotEmpty().WithMessage("Email or Phone is required.");

            RuleFor(x => x.EmailOrPhone)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid Email format.")
                .When(x => x.EmailOrPhone.Contains("@"));

            RuleFor(x => x.EmailOrPhone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid Phone number format.")
                .When(x => !x.EmailOrPhone.Contains("@"));

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmedPassword)
                .NotEmpty().WithMessage("Confirmed password is required.")
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
        }
    }
}
