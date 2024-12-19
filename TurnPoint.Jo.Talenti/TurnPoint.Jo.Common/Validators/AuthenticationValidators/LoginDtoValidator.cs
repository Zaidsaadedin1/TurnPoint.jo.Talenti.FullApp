using FluentValidation;
using TurnPoint.Jo.Common.Common.UserDtos;

namespace TurnPoint.Jo.Common.Validators.AuthenticationValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.EmailOrPhone)
                .NotEmpty().WithMessage("Email or Phone is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
