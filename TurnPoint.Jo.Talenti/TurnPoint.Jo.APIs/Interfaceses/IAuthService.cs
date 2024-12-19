using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Enums;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<string?> LoginUserAsync(LoginUserDto loginUserDto);
        Task<bool> IsUsersPhoneOrEmailTakenAsync(string emailOrPhone);
        Task<bool> UserPasswordResetAsync(ResetPasswordDto resetPasswordDto);
    }
}
