using IMS.Domain.Common.Enums;

namespace IMS.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }

        public Payment? Payment{ get; set; }
    }
}
