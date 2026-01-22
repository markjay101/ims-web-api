using IMS.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace IMS.Domain.Entities
{
    public class InternetPlan : BaseAuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int SpeedMbps { get; set; }
        [Precision(18, 2)] public decimal Price { get; set; }
        public string Currency { get; set; } = "PHP"; // for now PHP only is supported
    }
}
