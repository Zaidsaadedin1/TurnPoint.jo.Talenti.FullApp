using TurnPoint.Jo.APIs.Enums;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IEmailService
    {
        Task<EmailResponseStatus> SendEmailToUserAsync(string toEmail, string subject, string body);
    }
}
