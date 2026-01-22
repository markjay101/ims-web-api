using IMS.Domain.Common;

namespace IMS.Domain.Entities
{
    public class Document : BaseAuditableEntity
    {
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[]? Content { get; set; }
    }
}
