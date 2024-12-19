using TurnPoint.Jo.Common.Common.UserDtos;

namespace TurnPoint.Jo.Common.Interfaceses
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterUserDto registerUserDto);
        Task<string> LoginAsync(LoginUserDto loginUserDto);
        Task<bool> IsPhoneOrEmailTakenAsync(string emailOrPhone);
    }
}
