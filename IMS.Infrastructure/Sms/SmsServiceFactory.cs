using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IMS.Infrastructure.Sms
{
    internal class SmsServiceFactory(IServiceProvider serviceProvider)
    {
        public ISmsService GetSmsService()
        {
            var options = serviceProvider.GetRequiredService<IOptions<SmsOptions>>().Value;

            return options.Provider.ToLower() switch
            {
                "twilio" => (ISmsService)serviceProvider.GetRequiredService<TwilioService>(),
                "infobip" => (ISmsService)serviceProvider.GetRequiredService<InfobipService>(),
                _ => throw new InvalidOperationException($"SMS provider '{options.Provider}' is not supported.")
            };
        }
    }
}
