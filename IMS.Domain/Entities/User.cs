using IMS.Domain.Common;
using IMS.Domain.Common.Enums;

namespace IMS.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public Role Role { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
