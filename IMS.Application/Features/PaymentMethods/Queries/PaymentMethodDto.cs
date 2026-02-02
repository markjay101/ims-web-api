using IMS.Domain.Common.Enums;

namespace IMS.Application.Features.PaymentMethods.Queries
{
    public class PaymentMethodDto
    {
        public Guid Id { get; set; }
        public string MethodName { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
    }
}
