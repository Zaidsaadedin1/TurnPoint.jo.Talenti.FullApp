namespace TurnPoint.Jo.APIs.Common.AuthDtos
{
    public class LoginUserDto
    {
        public string EmailOrPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
