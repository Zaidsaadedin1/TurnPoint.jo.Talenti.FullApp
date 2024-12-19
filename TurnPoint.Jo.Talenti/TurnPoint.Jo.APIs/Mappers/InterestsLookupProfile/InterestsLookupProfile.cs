using AutoMapper;
using TurnPoint.Jo.APIs.Common.InterestDtos;
using TurnPoint.Jo.APIs.Entities;

namespace TurnPoint.Jo.APIs.Mappers.InterestsLookupProfile
{
    public class InterestsLookupProfile :Profile
    {
        public InterestsLookupProfile()
        {
            CreateMap<InterestsLookup, GetInterestDto>();
        }
    }
}
