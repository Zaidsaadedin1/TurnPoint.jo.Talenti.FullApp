namespace TurnPoint.Jo.APIs.Common.ProfileDtos
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public List<int> InterestIds { get; set; } = null!;

    }
}
