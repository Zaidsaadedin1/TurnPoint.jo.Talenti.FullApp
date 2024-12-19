using AutoMapper;
using TurnPoint.Jo.Common.Common.UserDtos;
using TurnPoint.Jo.Common.Entities;

namespace TurnPoint.Jo.Common.Mappers.UserProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Register DTO to Entity mappings
            CreateMap<RegisterUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // We'll hash the password separately
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Map User entity to RegisterDto
            CreateMap<User, RegisterUserDto>();
        }
    }
}
