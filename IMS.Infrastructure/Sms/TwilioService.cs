using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace IMS.Infrastructure.Sms
{
    internal class TwilioService : ISmsService
    {
        private readonly TwilioOptions _twilioSettings;
        private readonly ILogger<TwilioService> _logger;

        public TwilioService(ILogger<TwilioService> logger, IOptions<SmsOptions> smsOptions)
        {
            _logger = logger;
            _twilioSettings = smsOptions.Value.Twilio;

            TwilioClient.Init(_twilioSettings.AccountSID, _twilioSettings.AuthToken);
        }
        public async Task SendSmsAsync(string to, string from, string message, CancellationToken cancellation = default)
        {
            var result = await MessageResource.CreateAsync(
                body: message,
                to: new PhoneNumber(to), 
                from: new PhoneNumber(_twilioSettings.From), 
                messagingServiceSid: _twilioSettings.MessagingServiceSID);

            _logger.LogInformation("SMS message sent to {to}", to);
        }
    }
}
