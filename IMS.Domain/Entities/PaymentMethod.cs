using IMS.Domain.Common;
using IMS.Domain.Common.Enums;

namespace IMS.Domain.Entities
{
    public class PaymentMethod : BaseAuditableEntity
    {
        public PaymentMethodEnum MethodName { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
    }
}
