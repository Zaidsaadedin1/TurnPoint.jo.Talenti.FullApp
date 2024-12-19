using SendGrid;
using SendGrid.Helpers.Mail;
using TurnPoint.Jo.APIs.Enums;
using TurnPoint.Jo.APIs.Interfaceses;

namespace TurnPoint.Jo.APIs.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task<EmailResponseStatus> SendEmailToUserAsync(string toEmail, string subject, string body)
        {
            try
            {
                // Initialize the SendGrid client with the API key
                var client = new SendGridClient(_apiKey);

                // Define the sender and recipient information
                var from = new EmailAddress("zaidsaadedin10@gmail.com", "Talenti");
                var to = new EmailAddress(toEmail);

                // Create the email message
                var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

                // Send the email
                var response = await client.SendEmailAsync(msg);

                // Map the SendGrid response status to the custom EmailResponseStatus enum
                return response.StatusCode switch
                {
                    System.Net.HttpStatusCode.OK => EmailResponseStatus.Success,
                    System.Net.HttpStatusCode.Accepted => EmailResponseStatus.Success,
                    System.Net.HttpStatusCode.Unauthorized => EmailResponseStatus.Unauthorized,
                    System.Net.HttpStatusCode.BadRequest => EmailResponseStatus.InvalidRequest,
                    _ => EmailResponseStatus.UnknownError
                };
            }
            catch (Exception ex)
            {
                // Log the exception (can use ILogger if available)
                Console.WriteLine($"Exception occurred while sending email: {ex.Message}");

                // Return UnknownError for unhandled exceptions
                return EmailResponseStatus.UnknownError;
            }
        }
    }
}
