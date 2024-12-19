using TurnPoint.Jo.APIs.Common.InterestDtos;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IInterestsLookupUserService
    {
        Task<bool> AddInterestsToUserAsync(int userId, List<int> newInterests);
        Task<bool> RemoveInterestsFromUserAsync(int userId, List<int> newInterests);
        Task<List<GetInterestDto>> GetUserInterestsAsync(int userId);
    }
}
