using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Common.ProfileDtos;
using TurnPoint.Jo.APIs.Entities;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IUserService
    {
        Task<GetUserDto?> GetUserByEmailOrPhoneAsync(string emailOrPhone);
        Task<GetUserDto?> GetUserByIdAsync(int userId);
        Task<IEnumerable<GetUserDto>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> IsEmailOrPhoneUserTakenAsync(string emailOrPhone);
        Task<bool> AssignRoleToUserAsync(int userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(int userId, string roleName);
    }
}
