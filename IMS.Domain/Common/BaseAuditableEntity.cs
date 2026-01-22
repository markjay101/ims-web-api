using IMS.Domain.Common.Interfaces;

namespace IMS.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IBaseAuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
