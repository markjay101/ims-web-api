using IMS.Domain.Common;
using IMS.Domain.Common.Enums;

namespace IMS.Domain.Entities
{
    public class Customer : BaseAuditableEntity
    {
        public Guid? UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string Email { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public CustomerStatus Status { get; set; }
        public Guid? ModemId { get; set; }

        public Application Application { get; set; } = null!;
        public User? User { get; set; }
        public CustomerPlan Plan { get; set; } = null!;
        public Modem? Modem { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = [];
    }
}
