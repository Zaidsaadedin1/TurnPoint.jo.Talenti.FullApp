using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Entities;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IInterestsService
    {
        Task<IEnumerable<GetInterestDto>> GetAllInterestsAsync();
        Task<GetInterestDto?> GetInterestByIdAsync(int interestId);
        Task<bool> AddInterestAsync(string interestName);
        Task<bool> UpdateInterestAsync(int interestId, string updatedInterestName);
        Task<bool> DeleteInterestAsync(int interestId);
    }
}
