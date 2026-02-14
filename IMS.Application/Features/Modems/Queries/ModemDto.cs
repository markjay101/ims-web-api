using IMS.Application.Features.Customers.Queries;

namespace IMS.Application.Features.Modems.Queries
{
    public class ModemDto
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;

        public CustomerDto? Customer { get; set; }
    }
}
