using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Common.Shared;
using TurnPoint.Jo.APIs.Enums;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IAuthService
    {
        Task<GenericResponse<RegisterUserDto>> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<GenericResponse<string>> LoginUserAsync(LoginUserDto loginUserDto);
        Task<bool> IsUsersPhoneOrEmailTakenAsync(string emailOrPhone);
        Task<GenericResponse<bool>> UserPasswordResetAsync(ResetPasswordDto resetPasswordDto);
    }
}
