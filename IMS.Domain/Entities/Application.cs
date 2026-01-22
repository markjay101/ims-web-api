using IMS.Domain.Common;
using IMS.Domain.Common.Enums;

namespace IMS.Domain.Entities
{
    public class Application : BaseAuditableEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public ApplicationStatus Status { get; set; }
        public Guid InternetPlanId { get; set; }

        public ICollection<Document> Documents { get; set; } = null!;
        public InternetPlan InternetPlan { get; set; } = null!;
    }
}
