using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;

namespace TurnPoint.Jo.APIs.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<Role> _roleManager;

        public AuthService(
     IMapper mapper,
     IConfiguration configuration,
     IPasswordHasher<User> passwordHasher,
     ILogger<AuthService> logger,
     UserManager<User> userManager,
     IUserService userService,
     RoleManager<Role> roleManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            if (await IsUsersPhoneOrEmailTakenAsync(registerUserDto.Email))
            {
                _logger.LogWarning("Email {Email} is already taken.", registerUserDto.Email);
                return false;
            }

            var user = _mapper.Map<User>(registerUserDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Error creating user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return false;
            }

            await EnsureRolesExist();

            await _userManager.AddToRoleAsync(user, "User");

            return true;
        }
        public async Task<string?> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _userManager.Users
                              .FirstOrDefaultAsync(u => u.PhoneNumber == loginUserDto.EmailOrPhone || u.Email == loginUserDto.EmailOrPhone);
            if (user == null)
            {
                _logger.LogWarning("Login failed for email/phone: {EmailOrPhone}. User not found.", loginUserDto.EmailOrPhone);
                return null;
            }
            else if (user.EmailConfirmed || user.PhoneNumberConfirmed == false)
            {
                _logger.LogWarning("Login failed for email/phone: {EmailOrPhone}. User not found.", loginUserDto.EmailOrPhone);
                return null;
            }

            var userDto = _mapper.Map<User>(user);

            if (_passwordHasher.VerifyHashedPassword(user, userDto.PasswordHash, loginUserDto.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed for email/phone: {EmailOrPhone}. Incorrect password.", loginUserDto.EmailOrPhone);
                return null;
            }

            return await GenerateJwtToken(userDto);
        }
        public async Task<bool> IsUsersPhoneOrEmailTakenAsync(string emailOrPhone)
        {
            if (string.IsNullOrWhiteSpace(emailOrPhone))
            {
                _logger.LogWarning("Email or phone cannot be null or empty.", nameof(emailOrPhone));
                return false;
            }

            var user = await _userService.GetUserByEmailOrPhoneAsync(emailOrPhone);
            return user != null;
        }
        public async Task<bool> UserPasswordResetAsync(ResetPasswordDto resetPasswordDto)
        { 

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == resetPasswordDto.EmailOrPhone || u.Email == resetPasswordDto.EmailOrPhone);

            if (user == null)
            {
                _logger.LogWarning("User with email or phone {EmailOrPhone} not found.", resetPasswordDto.EmailOrPhone);
                return false;
            }

            return true;
        }
        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                _logger.LogWarning("JWT secret key is not configured.");
                throw new ArgumentNullException(nameof(secretKey), "JWT secret key is missing.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task EnsureRolesExist()
        {
            var roles = new[] { "Admin", "User", "Moderator" };
            foreach (var role in roles)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new Role { Name = role });
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogError("Error creating role {Role}.", role);
                    }
                }
            }
        }
    }
}
