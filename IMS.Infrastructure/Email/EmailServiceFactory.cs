using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Common.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IMS.Infrastructure.Email
{
    internal class EmailServiceFactory(IServiceProvider serviceProvider)
    {
        public IEmailService GetEmailService()
        {
            var options = serviceProvider.GetRequiredService<IOptions<EmailOptions>>().Value;

            return options.Provider.ToLower() switch
            {
                "gmail" => (IEmailService)serviceProvider.GetRequiredService<GmailService>(),
                _ => throw new InvalidOperationException($"Email provider '{options.Provider}' is not supported.")
            };
        }
    }
}
