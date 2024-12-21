using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IInterestsService
    {
        Task<GenericResponse<IEnumerable<GetInterestDto>>> GetAllInterestsAsync();
        Task<GenericResponse<GetInterestDto>> GetInterestByIdAsync(int interestId);
        Task<GenericResponse<bool>> AddInterestAsync(string interestName);
        Task<GenericResponse<bool>> UpdateInterestAsync(int interestId, string updatedInterestName);
        Task<GenericResponse<bool>> DeleteInterestAsync(int interestId);
    }
}
