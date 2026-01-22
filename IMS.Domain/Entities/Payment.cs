using IMS.Domain.Common.Enums;

namespace IMS.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public PaymentMethodEnum Method { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
