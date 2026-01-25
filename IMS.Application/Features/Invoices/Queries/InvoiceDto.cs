using IMS.Application.Features.Payments.Queries;
using IMS.Domain.Common.Enums;

namespace IMS.Application.Features.Invoices.Queries
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public DateTime IssueDate { get; set; }

        public PaymentDto? Payment { get; set; }
    }
}
