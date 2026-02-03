using IMS.Domain.Common;

namespace IMS.Domain.Entities
{
    public class Modem : BaseAuditableEntity
    {
        public string Model { get; set; } = null!;
        public string SerialNumber { get; set; } = null!;
        public string MacAddress { get; set; } = null!;

        public Customer? Customer { get; set; }
    }
}
