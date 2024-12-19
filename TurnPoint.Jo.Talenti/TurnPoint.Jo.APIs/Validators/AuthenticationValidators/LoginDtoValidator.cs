
using FluentValidation;
using TurnPoint.Jo.APIs.Common.AuthDtos;

namespace TurnPoint.Jo.APIs.Validators.AuthenticationValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.EmailOrPhone)
             .NotEmpty().WithMessage("Email is required.")
             .EmailAddress().WithMessage("Invalid email format.")
             .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email format is incorrect.");



            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required.")
               .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
               .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
               .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
               .Matches(@"\d").WithMessage("Password must contain at least one number.")
               .Matches(@"[^\w\d\s]").WithMessage("Password must contain at least one special character.");
        }
    }
}
