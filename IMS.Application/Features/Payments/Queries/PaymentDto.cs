using IMS.Domain.Common.Enums;

namespace IMS.Application.Features.Payments.Queries
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public string Method { get; set; } = null!;
        public string ReferenceNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
