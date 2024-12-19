
using FluentValidation;
using TurnPoint.Jo.APIs.Common.AuthDtos;

namespace TurnPoint.Jo.APIs.Validators.AuthenticationValidators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(30).WithMessage("Username cannot exceed 30 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email format is incorrect.");


            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+\d{10,15}$").WithMessage("Invalid phone number format. Include the country code (e.g., +1234567890).");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[^\w\d\s]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be 'Male', 'Female'");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .Must(dob => dob.Value <= DateTime.UtcNow.AddYears(-13))
                .WithMessage("User must be at least 13 years old.");

            RuleFor(x => x.InterestIds)
                .NotNull().WithMessage("At least one interest must be selected.")
                .Must(ids => ids.Count > 0).WithMessage("At least one interest must be selected.");
        }
    }

}
