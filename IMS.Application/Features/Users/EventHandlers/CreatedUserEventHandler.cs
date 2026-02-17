using IMS.Application.Common.Interfaces;
using IMS.Domain.Events;
using MediatR;

namespace IMS.Application.Features.Users.EventHandlers
{
    public class CreatedUserEventHandler(IEmailService emailService) : INotificationHandler<CreateUserEvent>
    {
        public async Task Handle(CreateUserEvent notification, CancellationToken cancellationToken)
        {
            await emailService.SendEmailAsync(
                to: notification.User.Email!,
                subject: "Welcome to IMS",
                body: $"Hello {notification.User.FirstName},\n\nYour account has been created successfully.\nPlease use this password '{notification.User.Password}'.\n\nBest regards,\nIMS Team"
            );
        }
    }
}
