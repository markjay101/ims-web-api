namespace IMS.Domain.Common.Interfaces
{
    public interface IBaseEntity
    {
        Guid Id { get; }
        IReadOnlyList<BaseEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
