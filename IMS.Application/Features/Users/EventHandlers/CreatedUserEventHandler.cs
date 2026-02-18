using IMS.Application.Common.Interfaces;
using IMS.Domain.Events;
using MediatR;

namespace IMS.Application.Features.Users.EventHandlers
{
    public class CreatedUserEventHandler(IEmailService emailService, IEmailTemplateService emailTemplateService) : INotificationHandler<CreatedUserEvent>
    {
        public async Task Handle(CreatedUserEvent notification, CancellationToken cancellationToken)
        {
            var rawTemplate = await emailTemplateService.GetRawTemplateAsync("CreatedUser", cancellationToken);
            var placeHolders = new Dictionary<string, string>
            {
                {"FirstName", notification.User.FirstName },
                {"Password", notification.User.Password! },
            };

            var body = emailTemplateService.ReplacePlaceholders(rawTemplate, placeHolders);

            await emailService.SendEmailAsync(
                to: notification.User.Email!,
                subject: "Welcome to IMS",
                body: body,
                cancellationToken
            );
        }
    }
}
