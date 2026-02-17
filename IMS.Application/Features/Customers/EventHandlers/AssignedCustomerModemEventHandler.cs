using IMS.Application.Common.Interfaces;
using IMS.Domain.Events;
using MediatR;

namespace IMS.Application.Features.Customers.EventHandlers
{
    public class AssignedCustomerModemEventHandler(IEmailService emailService) : INotificationHandler<AssignedCustomerModemEvent>
    {
        public async Task Handle(AssignedCustomerModemEvent notification, CancellationToken cancellationToken)
        { 

            var customer = notification.Customer;

            var body = $@"
                <h3>Hello {customer.Application.FirstName},</h3>
                <p>Great news! Your modem installation is complete, and your internet subscription is now <strong>active</strong>.</p>
                <p>You can start using your connection immediately. Welcome to the IMS network!</p>
                <hr />
                <p><strong>Account Details:</strong></p>
                <ul>
                    <li><strong>Status:</strong> Active</li>
                    <li><strong>Activation Date:</strong> {customer.Plan.StartDate:MMMM dd, yyyy}</li>
                    <li><strong>Next Billing Date:</strong> {customer.Plan.NextDueDate:MMMM dd, yyyy}</li>
                </ul>
                <p>If you have any issues with your connection, please contact our support team.</p>
                <p>Best regards,<br />IMS Team</p>";

            await emailService.SendEmailAsync(
                to: customer.Email,
                subject: "Your Internet Service is Now Active!",
                body: body,
                cancellationToken
            );
        }
    }
}
