using IMS.Application.Common.Interfaces;
using IMS.Domain.Events;
using MediatR;

namespace IMS.Application.Features.Customers.EventHandlers
{
    public class AssignedCustomerModemEventHandler(IEmailService emailService, IEmailTemplateService emailTemplateService) : INotificationHandler<AssignedCustomerModemEvent>
    {
        public async Task Handle(AssignedCustomerModemEvent notification, CancellationToken cancellationToken)
        { 

            var customer = notification.Customer;

            var rawTemplate = await emailTemplateService.GetRawTemplateAsync("AssignedCustomerModem", cancellationToken);
            var placeHolders = new Dictionary<string, string>
            {
                { "FirstName", customer.Application.FirstName },
                { "StartDate", customer.Plan.StartDate!.Value.ToString("MMMM dd, yyyy") },
                { "NextDueDate", customer.Plan.NextDueDate!.Value.ToString("MMMM dd, yyyy") }
            };

            var body = emailTemplateService.ReplacePlaceholders(rawTemplate, placeHolders);

            await emailService.SendEmailAsync(
                to: customer.Email,
                subject: "Your Internet Service is Now Active!",
                body: body,
                cancellationToken
            );
        }
    }
}
