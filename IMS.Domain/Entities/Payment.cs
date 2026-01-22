using IMS.Domain.Common;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid InvoiceId { get; set; }
        public PaymentMethodEnum Method { get; set; }
        public string ReferenceNumber { get; set; } = null!;
        public DateTime PaymentDate { get; set; }
        [Precision(18,2)] public decimal Amount { get; set; }
    }
}
