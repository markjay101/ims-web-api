
namespace IMS.Domain.Common.Interfaces
{
    public interface IBaseAuditableEntity
    {
        DateTime CreatedAt { get; }
        string? CreatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
        string? UpdatedBy { get; set; }
    }
}
