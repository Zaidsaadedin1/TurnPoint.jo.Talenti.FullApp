using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;
using TernPoint.Jo.Talenti.DatabaseManager;
using TurnPoint.Jo.APIs.Common.InterestDtos;

namespace TurnPoint.Jo.APIs.Services
{
    public class InterestsService : IInterestsService
    {
        private readonly ApplicationDbContext _dbContext;

        public InterestsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetInterestDto>> GetAllInterestsAsync()
        {
            return await _dbContext.Interests
                .Select(i => new GetInterestDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync(); 
        }

  
        public async Task<GetInterestDto?> GetInterestByIdAsync(int interestId)
        {
            return await _dbContext.Interests
                .Where(i => i.Id == interestId)
                .Select(i => new GetInterestDto
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .FirstOrDefaultAsync(); 
        }

        public async Task<bool> AddInterestAsync(string interestName)
        {
            var existingInterest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Name == interestName);

            if (existingInterest != null)
            {
                return false;
            }

            _dbContext.Interests.Add(new InterestsLookup { Name = interestName });
            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task<bool> UpdateInterestAsync(int interestId, string updatedInterestName)
        {
            var existingInterest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Name == updatedInterestName);

            if (existingInterest != null)
            {
                return false;
            }

            var interest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Id == interestId);

            if (interest == null)
                return false;

            interest.Name = updatedInterestName;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInterestAsync(int interestId)
        {
            var interest = await _dbContext.Interests
                .FirstOrDefaultAsync(i => i.Id == interestId);

            if (interest == null)
                return false;

            _dbContext.Interests.Remove(interest);
            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
