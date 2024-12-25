using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Common.Shared;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.Talenti.SharedModels.Entities;

namespace TurnPoint.Jo.APIs.Services
{
    public class InterestsLookupUserService : IInterestsLookupUserService
    {
        private readonly ILogger<InterestsLookupUserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public InterestsLookupUserService(ILogger<InterestsLookupUserService> logger, UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<GenericResponse<bool>> AddInterestsToUserAsync(int userId, List<int> newInterests)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for adding interests.", userId);
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var existingInterests = await _dbContext.InterestsLookupUsers
                .Where(i => i.UserId == userId)
                .Select(i => i.InterestId)
                .ToListAsync();

            var interestsToAdd = newInterests.Where(interestId => !existingInterests.Contains(interestId)).ToList();

            if (!interestsToAdd.Any())
            {
                _logger.LogInformation("User with ID {UserId} already has all the provided interests.", userId);
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "user already have this interests."
                };
            }

            var userInterests = interestsToAdd.Select(interestId => new InterestsLookupUser
            {
                UserId = userId,
                InterestId = interestId
            }).ToList();

            _dbContext.InterestsLookupUsers.AddRange(userInterests);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Added {InterestCount} new interests to user with ID {UserId}.", interestsToAdd.Count, userId);
            return new GenericResponse<bool>
            {
                Success = true,
                Message = "Interests added successfully.",
                Data = true
            };
        }

        public async Task<GenericResponse<bool>> RemoveInterestsFromUserAsync(int userId, List<int> interestIds)
        {
            var interestsLookupUser = await _dbContext.InterestsLookupUsers
                .Where(ui => ui.UserId.Equals(userId) && interestIds.Contains(ui.InterestId))
                .ToListAsync();

            if (interestsLookupUser.Count == 0)
            {
                _logger.LogWarning("No interests found for user with ID {UserId} to remove.", userId);
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "No interests found to remove."
                };
            }

            _dbContext.InterestsLookupUsers.RemoveRange(interestsLookupUser);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Removed {InterestCount} interests from user with ID {UserId}.", interestsLookupUser.Count, userId);
            return new GenericResponse<bool>
            {
                Success = true,
                Message = "Interests removed successfully.",
                Data = true
            };
        }

        public async Task<GenericResponse<List<GetInterestDto>>> GetUserInterestsAsync(int userId)
        {
            var userInterests = await _dbContext.InterestsLookupUsers
                .Where(ui => ui.UserId == userId)
                .Include(ui => ui.Interest)
                .Select(ui => new GetInterestDto
                {
                    Id = ui.Interest.Id,
                    Name = ui.Interest.Name
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {InterestCount} interests for user with ID {UserId}.", userInterests.Count, userId);

            return new GenericResponse<List<GetInterestDto>>
            {
                Success = true,
                Message = "User interests retrieved successfully.",
                Data = userInterests
            };
        }
    }
}


