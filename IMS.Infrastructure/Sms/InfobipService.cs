using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Common.Options;
using Infobip.Api.Client;
using Infobip.Api.Client.Api;
using Infobip.Api.Client.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IMS.Infrastructure.Sms
{
    internal class InfobipService(ILogger<InfobipService> logger, IOptions<SmsOptions> smsOptions) : ISmsService
    {
        private readonly InfobipOptions _infobipSettings = smsOptions.Value.Infobip;

        public async Task SendSmsAsync(string to, string from, string message, CancellationToken cancellation = default)
        {
            var configuration = new Configuration()
            {
                BasePath = _infobipSettings.BaseUrl,
                ApiKey = _infobipSettings.ApiKey
            };

            var smsApi = new SmsApi(configuration);

            var smsMessage = new SmsMessage(
                sender: from,
                destinations: [ new(to: to) ],
                content: new SmsMessageContent(new SmsTextContent(text: message))
            );

            var smsRequest = new SmsRequest(
                messages: [ smsMessage ]
            );

            var response = await smsApi.SendSmsMessagesAsync(smsRequest);

            logger.LogInformation("SMS message sent to {to}", to);
        }
    }
}
