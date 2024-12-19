using RestSharp;
using TurnPoint.Jo.APIs.Enums;
using TurnPoint.Jo.APIs.Interfaceses;

namespace TurnPoint.Jo.APIs.Services
{
    public class SmsService : ISmsService
    {
        private readonly string _apiKey;
        private readonly string _fromPhoneNumber;
        private readonly string _apiBaseUrl;

        public SmsService(IConfiguration configuration)
        {
            // Retrieve values from configuration
            _apiKey = configuration.GetValue<string>("infobip:ApiKey")
                      ?? throw new ArgumentNullException(nameof(_apiKey), "Infobip ApiKey is not configured.");
            _fromPhoneNumber = configuration.GetValue<string>("infobip:FromPhoneNumber")
                               ?? throw new ArgumentNullException(nameof(_fromPhoneNumber), "Infobip FromPhoneNumber is not configured.");
            _apiBaseUrl = configuration.GetValue<string>("infobip:APIBaseURL")
                          ?? throw new ArgumentNullException(nameof(_apiBaseUrl), "Infobip APIBaseURL is not configured.");
            Console.WriteLine($"ApiKey: {configuration.GetValue<string>("infobip:ApiKey")}");
            Console.WriteLine($"FromPhoneNumber: {configuration.GetValue<string>("infobip:FromPhoneNumber")}");
            Console.WriteLine($"APIBaseURL: {configuration.GetValue<string>("infobip:APIBaseURL")}");

        }


        public async Task<SmsResponseStatus> SendSmsToUserAsync(string phoneNumber, string message, string otp)
        {
            try
            {
                // Set up the RestClient with the base URL
                var options = new RestClientOptions($"https://{_apiBaseUrl}")
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest("/sms/2/text/advanced", Method.Post);

                // Set headers
                request.AddHeader("Authorization", $"App {_apiKey}");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");

                // Use the phone number and message in the request body
                var body = $@"{{
                        ""messages"":[
                            {{
                                ""from"":""{_fromPhoneNumber}"",
                                ""destinations"":[
                                    {{
                                        ""to"":""{phoneNumber}""
                                    }}
                                ],
                                  ""text"":""{message} : {otp}""
                            }}
                        ]
                    }}";

                // Add the JSON body to the request
                request.AddJsonBody(body);

                // Send the request asynchronously
                RestResponse response = await client.ExecuteAsync(request);

                // Check if the request was successful
                if (response.IsSuccessful)
                {
                    return SmsResponseStatus.Success;
                }
                else
                {
                    Console.WriteLine($"Infobip API error: {response.Content}");
                    return SmsResponseStatus.FailedToSend;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS sending error: {ex.Message}");
                return SmsResponseStatus.UnknownError;
            }
        }
    }
}
