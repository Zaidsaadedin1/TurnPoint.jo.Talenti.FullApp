using TurnPoint.Jo.APIs.Common.VerificationsDtos;
using TurnPoint.Jo.APIs.Enums;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface ISmsService
    {
        Task<SmsResponseStatus> SendSmsToUserAsync(string phoneNumber, string message, string otp);
    }
}
