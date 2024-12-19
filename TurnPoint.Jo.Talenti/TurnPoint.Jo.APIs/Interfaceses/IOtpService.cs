using TurnPoint.Jo.APIs.Common.VerificationsDtos;
using TurnPoint.Jo.APIs.Enums;

namespace TurnPoint.Jo.APIs.Interfaceses
{
    public interface IOtpService
    {
        Task<bool> SendOtpAsync(string emailOrPhone);
        Task<VerificationResult> VerifyUserOtpAsync(VerifyOtpDto verifyOtpDto);
    }
}
