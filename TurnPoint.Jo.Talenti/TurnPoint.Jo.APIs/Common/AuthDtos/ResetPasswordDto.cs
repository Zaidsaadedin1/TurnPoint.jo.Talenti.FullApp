namespace TurnPoint.Jo.APIs.Common.AuthDtos
{
    public class ResetPasswordDto
    {
        public string EmailOrPhone { get; set; } = null!;
        public string Opt { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmedPassword { get; set; } = null!;

    }
}
