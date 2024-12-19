using TurnPoint.Jo.APIs.Common.InterestDtos;

namespace TurnPoint.Jo.APIs.Common.ProfileDtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<GetInterestDto> UserInterests { get; set; } = null!;
    }
}
