using AutoMapper;
using TurnPoint.Jo.APIs.Common.ProfileDtos;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Common.InterestDtos;
using TernPoint.Jo.Talenti.DatabaseManager;
using Microsoft.Extensions.Logging;

namespace TurnPoint.Jo.APIs.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IInterestsLookupUserService _userInterestsService;

        public UserService(ILogger<UserService> logger, UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager, ApplicationDbContext dbContext, IInterestsLookupUserService userInterestsService)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _userInterestsService = userInterestsService;
        }

        public async Task<GetUserDto?> GetUserByEmailOrPhoneAsync(string emailOrPhone)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == emailOrPhone || u.PhoneNumber == emailOrPhone);

            if (user == null)
            {
                _logger.LogWarning("User with email/phone {EmailOrPhone} not found.", emailOrPhone);
                return null;
            }

            _logger.LogInformation("User with email/phone {EmailOrPhone} found.", emailOrPhone);

            var userDto = _mapper.Map<GetUserDto>(user);

            userDto.UserInterests = await _userInterestsService.GetUserInterestsAsync(userDto.Id);

            return userDto;
        }

        public async Task<GetUserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                return null;
            }

            _logger.LogInformation("User with ID {UserId} found.", userId);

            var userDto = _mapper.Map<GetUserDto>(user);

            userDto.UserInterests = await _userInterestsService.GetUserInterestsAsync(userId);

            return userDto;
        }

        public async Task<IEnumerable<GetUserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            _logger.LogInformation("Retrieved {UserCount} users.", users.Count);

            var userDtos = _mapper.Map<IEnumerable<GetUserDto>>(users);

            foreach (var userDto in userDtos)
            {
                userDto.UserInterests = await _userInterestsService.GetUserInterestsAsync(userDto.Id);
            }

            return userDtos;
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update.", userId);
                return false;
            }

            _mapper.Map(userDto, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Error updating user with ID {UserId}: {Errors}", userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            _logger.LogInformation("Updated user with ID {UserId}.", userId);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion.", userId);
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Error deleting user with ID {UserId}: {Errors}", userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            _logger.LogInformation("Deleted user with ID {UserId}.", userId);
            return true;
        }

        public async Task<bool> IsEmailOrPhoneUserTakenAsync(string emailOrPhone)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == emailOrPhone || u.PhoneNumber == emailOrPhone);
            return user != null;
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for role assignment.", userId);
                return false;
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                _logger.LogWarning("Role {RoleName} does not exist.", roleName);
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogError("Error assigning role {RoleName} to user with ID {UserId}: {Errors}", roleName, userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            _logger.LogInformation("Assigned role {RoleName} to user with ID {UserId}.", roleName, userId);
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for role removal.", userId);
                return false;
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                _logger.LogError("Error removing role {RoleName} from user with ID {UserId}: {Errors}", roleName, userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            _logger.LogInformation("Removed role {RoleName} from user with ID {UserId}.", roleName, userId);
            return true;
        }
 
    }
}
