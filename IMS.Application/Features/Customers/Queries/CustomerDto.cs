using IMS.Domain.Common.Enums;

namespace IMS.Application.Features.Customers.Queries
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Email { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid? ModemId { get; set; }
    }
}
