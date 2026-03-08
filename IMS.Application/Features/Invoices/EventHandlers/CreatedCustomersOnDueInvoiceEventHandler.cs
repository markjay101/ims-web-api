using IMS.Application.Common.Interfaces;
using IMS.Domain.Events;
using MediatR;

namespace IMS.Application.Features.Invoices.EventHandlers
{
    internal class CreatedCustomersOnDueInvoiceEventHandler(IEmailTemplateService emailTemplateService, IEmailService emailService) : INotificationHandler<CreatedCustomersOnDueInvoiceEvent>
    {
        public async Task Handle(CreatedCustomersOnDueInvoiceEvent notification, CancellationToken cancellationToken)
        {
            var template = await emailTemplateService.GetRawTemplateAsync("CreatedCustomerOnDueInvoice", cancellationToken);
            var dateToday = DateTime.UtcNow.Date;

            foreach (var customer in notification.Customers)
            {
                var placeHolders = new Dictionary<string, string>
                {
                    { "CustomerName", customer.Application.FirstName },
                    { "PlanName", customer.Plan.InternetPlan.Name },
                    { "DueDate", dateToday.ToString("MMMM dd, yyyy") },
                    { "Amount", customer.Plan.InternetPlan.Price.ToString("C") },
                };  

                var body = emailTemplateService.ReplacePlaceholders(template, placeHolders);
                var subject = $"Action Required: Your {dateToday.Month} Internet Invoice is Due";

                await emailService.SendEmailAsync(customer.Email, subject, body, cancellationToken);
            }
        }
    }
}
