using IMS.Application.Features.Modems.Queries;

namespace IMS.Application.Features.Customers.Queries
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Status { get; set; } = null!;

        public ModemDto? Modem { get; set; }
        public CustomerPlanDto Plan { get; set; } = new();
    }
}
