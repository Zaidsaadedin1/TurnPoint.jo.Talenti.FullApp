using AutoMapper;
using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Entities;

public class InterestsLookupProfile : Profile
{
    public InterestsLookupProfile()
    {
        // Map InterestsLookup to GetInterestDto
        CreateMap<InterestsLookup, GetInterestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
