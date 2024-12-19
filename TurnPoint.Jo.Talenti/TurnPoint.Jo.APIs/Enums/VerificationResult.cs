namespace TurnPoint.Jo.APIs.Enums
{
    public enum VerificationResult
    {
        Success = 1,        // OTP Verified successfully
        UserNotFound = 2,   // User not found
        OtpNotFound = 3,    // OTP not found for the user
        OtpExpired = 4,     // OTP has expired
        InvalidOtp = 5      // Invalid OTP entered
    }
}
