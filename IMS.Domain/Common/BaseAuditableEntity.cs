using System;
using System.Collections.Generic;
using System.Text;

namespace IMS.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
