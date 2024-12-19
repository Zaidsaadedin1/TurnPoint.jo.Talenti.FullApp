namespace TurnPoint.Jo.APIs.Common.VerificationsDtos
{
    public class VerifyOtpDto
    {
        public string Otp { get; set; } = null!;
        public string EmailOrPhone { get; set; } = null!;
    }
}
