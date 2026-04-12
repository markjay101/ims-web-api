using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Options;
using IMS.Domain.Events;
using MediatR;
using Microsoft.Extensions.Options;

namespace IMS.Application.Features.Applications.EventHandlers
{
    internal class CreatedApplicationEventHandler(IEmailService emailService, IEmailTemplateService emailTemplateService, IOptions<ClientsOption> options) : INotificationHandler<CreatedApplicationEvent>
    {
        private readonly ClientsOption clientsOption = options.Value;
        public async Task Handle(CreatedApplicationEvent notification, CancellationToken cancellationToken)
        {
            var rawTemplate = await emailTemplateService.GetRawTemplateAsync("CreatedApplication", cancellationToken);

            var placeHolders = new Dictionary<string, string>
            {
                { "FirstName", notification.Application.FirstName },
                { "Status", notification.Application.Status.ToString() },
                { "RedirectURL", clientsOption.Customer + "/application-status" }
            };

            var body = emailTemplateService.ReplacePlaceholders(rawTemplate, placeHolders);

            await emailService.SendEmailAsync(notification.Application.Email, "Your Application Has Been Successfully Created", body, cancellationToken);
        }
    }
}
