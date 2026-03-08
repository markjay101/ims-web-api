using IMS.Domain.Common;
using IMS.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.Domain.Entities
{
    public class Invoice : BaseAuditableEntity
    {
        public Guid CustomerId { get; set; }
        [Precision(18, 2)] public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }

        public Customer Customer { get; set; } = null!;
        public Payment? Payment{ get; set; }
    }
}
