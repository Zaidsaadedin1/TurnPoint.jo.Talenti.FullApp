using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Common.Shared;

namespace TurnPoint.Jo.APIs.Services
{
    public class InterestsService : IInterestsService
    {
        private readonly ApplicationDbContext _dbContext;

        public InterestsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GenericResponse<IEnumerable<GetInterestDto>>> GetAllInterestsAsync()
        {
            var interests = await _dbContext.Interests
                .Select(i => new GetInterestDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync();

            return new GenericResponse<IEnumerable<GetInterestDto>>
            {
                Success = true,
                Message = "Interests retrieved successfully.",
                Data = interests
            };
        }

        public async Task<GenericResponse<GetInterestDto>> GetInterestByIdAsync(int interestId)
        {
            var interest = await _dbContext.Interests
                .Where(i => i.Id == interestId)
                .Select(i => new GetInterestDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .FirstOrDefaultAsync();

            if (interest == null)
            {
                return new GenericResponse<GetInterestDto>
                {
                    Success = false,
                    Message = "Interest not found."
                };
            }

            return new GenericResponse<GetInterestDto>
            {
                Success = true,
                Message = "Interest retrieved successfully.",
                Data = interest
            };
        }

        public async Task<GenericResponse<bool>> AddInterestAsync(string interestName)
        {
            var existingInterest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Name == interestName);

            if (existingInterest != null)
            {
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Interest already exists."
                };
            }

            _dbContext.Interests.Add(new InterestsLookup { Name = interestName });
            await _dbContext.SaveChangesAsync();

            return new GenericResponse<bool>
            {
                Success = true,
                Message = "Interest added successfully.",
                Data = true
            };
        }

        public async Task<GenericResponse<bool>> UpdateInterestAsync(int interestId, string updatedInterestName)
        {
            var existingInterest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Name == updatedInterestName);

            if (existingInterest != null)
            {
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Interest with the updated name already exists."
                };
            }

            var interest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Id == interestId);

            if (interest == null)
            {
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Interest not found."
                };
            }

            interest.Name = updatedInterestName;
            await _dbContext.SaveChangesAsync();

            return new GenericResponse<bool>
            {
                Success = true,
                Message = "Interest updated successfully.",
                Data = true
            };
        }

        public async Task<GenericResponse<bool>> DeleteInterestAsync(int interestId)
        {
            var interest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Id == interestId);

            if (interest == null)
            {
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Interest not found."
                };
            }

            _dbContext.Interests.Remove(interest);
            await _dbContext.SaveChangesAsync();

            return new GenericResponse<bool>
            {
                Success = true,
                Message = "Interest deleted successfully.",
                Data = true
            };
        }
    }
}
