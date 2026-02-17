using IMS.Domain.Common;
using IMS.Domain.Entities;

namespace IMS.Domain.Events
{
    public class CreatedUserEvent : BaseEvent
    {
        public CreatedUserEvent(User user) => User = user;

        public User User { get; set; }
    }
}
