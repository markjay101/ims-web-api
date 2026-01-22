using IMS.Domain.Common;
using IMS.Domain.Common.Enums;
using IMS.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entities
{
    public class User : IdentityUser<Guid>, IBaseAuditableEntity, IBaseEntity
    {
        public Role Role { get; set; }
        [EmailAddress] public override string? UserName { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }


        private readonly List<BaseEvent> _domainEvents = [];

        [NotMapped]
        public IReadOnlyList<BaseEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(BaseEvent eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(BaseEvent eventItem)
        {
            _domainEvents.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
