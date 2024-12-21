using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Common.Shared;
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

        public async Task<GenericResponse<RegisterUserDto>> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var isUsersPhoneOrEmailTaken = await _userManager.Users
           .FirstOrDefaultAsync(u => u.Email == registerUserDto.Email || u.PhoneNumber == registerUserDto.PhoneNumber);

            if (isUsersPhoneOrEmailTaken == null)
            {
                _logger.LogWarning("Email or phone is already taken", registerUserDto.Email , registerUserDto.PhoneNumber);
                return new GenericResponse<RegisterUserDto>
                {
                    Success = false,
                    Message = "Email or Phone is already taken."
                };
            }

            var user = _mapper.Map<User>(registerUserDto);
            user.PasswordHash = _passwordHasher.HashPassword(user, registerUserDto.Password);

            var result = await _userManager.CreateAsync(user);

            await EnsureRolesExist();
            await _userManager.AddToRoleAsync(user, "User");

            return new GenericResponse<RegisterUserDto>
            {
                Success = true,
                Message = "User created successfully.",
                Data = registerUserDto
            };
        }

        public async Task<GenericResponse<string>> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == loginUserDto.EmailOrPhone || u.Email == loginUserDto.EmailOrPhone);

            if (user == null || (!user.EmailConfirmed && !user.PhoneNumberConfirmed))
            {
                _logger.LogWarning("Login failed for email or phone: {EmailOrPhone}. Confirmation issue.", loginUserDto.EmailOrPhone);
                return new GenericResponse<string>
                {
                    Success = false,
                    Message = "Login failed. Email or phone not confirmed."
                };
            }

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password) == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed for email/phone: {EmailOrPhone}. Incorrect password.", loginUserDto.EmailOrPhone);
                return new GenericResponse<string>
                {
                    Success = false,
                    Message = "Incorrect password."
                };
            }

            var token = await GenerateJwtToken(user);
            return new GenericResponse<string>
            {
                Success = true,
                Message = "Login successful.",
                Data = token
            };
        }

        public async Task<bool> IsUsersPhoneOrEmailTakenAsync(string emailOrPhone)
        {
            var user = await _userService.GetUserByEmailOrPhoneAsync(emailOrPhone);
            return user == null;
        }

        public async Task<GenericResponse<bool>> UserPasswordResetAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == resetPasswordDto.EmailOrPhone || u.Email == resetPasswordDto.EmailOrPhone);

            if (user == null)
            {
                _logger.LogWarning("User with email or phone not found: {EmailOrPhone}", resetPasswordDto.EmailOrPhone);
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "User with email or phone not found."
                };
            }

            if (resetPasswordDto.Opt != user.Otp)
            {
                return new GenericResponse<bool>
                {
                    Success = false,
                    Message = "The entered OTP is not correct."
                };
            }

            var newHashedPassword = _passwordHasher.HashPassword(user, resetPasswordDto.NewPassword);
            user.PasswordHash = newHashedPassword;

            user.Otp = null;
            user.OtpExpiresAt = null;

            var result = await _userManager.UpdateAsync(user);

            return new GenericResponse<bool>
            {
                Success = true,
                Message = "Password reset successfully.",
                Data = true
            };
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
                _logger.LogError("JWT secret key is missing.");
                throw new ArgumentNullException("JwtSettings:SecretKey", "JWT secret key is missing.");
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
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(new Role { Name = role });
                    if (!result.Succeeded)
                    {
                        _logger.LogError("Error creating role: {Role}", role);
                    }
                }
            }
        }
    }
}

