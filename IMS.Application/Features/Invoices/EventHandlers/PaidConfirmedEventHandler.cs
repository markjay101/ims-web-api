using IMS.Application.Common.Interfaces;
using IMS.Domain.Events;
using MediatR;

namespace IMS.Application.Features.Invoices.EventHandlers
{
    internal class PaidConfirmedEventHandler(IEmailTemplateService emailTemplateService, IEmailService emailService) : INotificationHandler<PaidConfirmedEvent>
    {
        public async Task Handle(PaidConfirmedEvent notification, CancellationToken cancellationToken)
        {
            var template = await emailTemplateService.GetRawTemplateAsync("PaidConfirmed", cancellationToken);

            var placeHolders = new Dictionary<string, string>
                {
                    { "CustomerName", notification.Invoice.Customer!.Application.FirstName },
                    { "ReferenceNo", notification.Invoice.Payment!.ReferenceNumber },
                    { "PlanName", notification.Invoice.Customer.Plan.InternetPlan.Name },
                    { "PaymentDate", notification.Invoice.Payment.PaymentDate.ToString("MMMM dd, yyyy") },
                    { "Amount", notification.Invoice.Payment.Amount.ToString("C") },
                };

            var body = emailTemplateService.ReplacePlaceholders(template, placeHolders);
            var subject = $"Payment Confirmation - {placeHolders["PlanName"]} (Ref: {placeHolders["ReferenceNo"]})";

            await emailService.SendEmailAsync(notification.Invoice.Customer.Application.Email, subject, body, cancellationToken);
        }
    }
}
