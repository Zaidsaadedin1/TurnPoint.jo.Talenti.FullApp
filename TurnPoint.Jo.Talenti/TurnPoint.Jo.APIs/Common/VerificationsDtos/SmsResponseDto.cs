using TurnPoint.Jo.APIs.Enums;

namespace TurnPoint.Jo.APIs.Common.VerificationsDtos
{
    public class SmsResponseDto
    {
        public SmsResponseStatus Status { get; set; }
        public string OtpCode { get; set; } = null!;
    }
}
