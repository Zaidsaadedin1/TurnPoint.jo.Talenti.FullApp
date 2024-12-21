using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IInterestsLookupUserService
    {
        Task<GenericResponse<bool>> AddInterestsToUserAsync(int userId, List<int> newInterests);
        Task<GenericResponse<bool>> RemoveInterestsFromUserAsync(int userId, List<int> interestIds);
        Task<GenericResponse<List<GetInterestDto>>> GetUserInterestsAsync(int userId);
    }
}