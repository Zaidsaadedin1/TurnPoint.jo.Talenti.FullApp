using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Common.ProfileDtos;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IUserService
    {
        Task<GenericResponse<GetUserDto>> GetUserByEmailOrPhoneAsync(string emailOrPhone);
        Task<GenericResponse<GetUserDto>> GetUserByIdAsync(int userId);
        Task<GenericResponse<IEnumerable<GetUserDto>>> GetAllUsersAsync();
        Task<GenericResponse<bool>> UpdateUserAsync(int userId, UpdateUserDto userDto);
        Task<GenericResponse<bool>> DeleteUserAsync(int userId);
        Task<GenericResponse<bool>> IsEmailOrPhoneUserTakenAsync(string emailOrPhone);
        Task<GenericResponse<bool>> AssignRoleToUserAsync(int userId, string roleName);
        Task<GenericResponse<bool>> RemoveRoleFromUserAsync(int userId, string roleName);
    }
}
