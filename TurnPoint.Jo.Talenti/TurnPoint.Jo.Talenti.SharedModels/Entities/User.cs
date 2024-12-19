using Microsoft.AspNetCore.Identity;
using TurnPoint.Jo.Talenti.SharedModels.Entities;

namespace TurnPoint.Jo.APIs.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

        public string? Otp { get; set; }
        public DateTime? OtpExpiresAt { get; set; }

        public List<InterestsLookupUser> Interests { get; set; } = new List<InterestsLookupUser>();
    }
}
