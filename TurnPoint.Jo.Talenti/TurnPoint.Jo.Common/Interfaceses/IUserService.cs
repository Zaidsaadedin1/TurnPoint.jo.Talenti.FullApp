using TurnPoint.Jo.Common.Entities;

namespace TurnPoint.Jo.Common.Interfaceses
{
    public interface IUserService
    {
        Task<User> GetByEmailOrPhoneAsync(string emailOrPhone);
        Task AddAsync(User user);
    }
}
