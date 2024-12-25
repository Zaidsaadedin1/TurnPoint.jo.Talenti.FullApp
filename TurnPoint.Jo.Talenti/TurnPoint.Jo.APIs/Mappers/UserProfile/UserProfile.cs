using AutoMapper;
using TurnPoint.Jo.APIs.Common.AuthDtos;
using TurnPoint.Jo.APIs.Common.ProfileDtos;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.Talenti.SharedModels.Entities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Mapping for RegisterUserDto to User
        CreateMap<RegisterUserDto, User>();
        
        // Mapping for User to GetUserDto
        CreateMap<User, GetUserDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserInterests, opt => opt.MapFrom(src =>
                src.Interests.Select(i => i.InterestId)));

        // Mapping for UpdateUserDto to User
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Interests, opt => opt.MapFrom(src =>
                src.InterestIds.Select(id => new InterestsLookupUser { InterestId = id })));
    }
}
