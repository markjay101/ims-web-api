using IMS.Domain.Common;
using IMS.Domain.Entities;

namespace IMS.Domain.Events
{
    public class CreateUserEvent : BaseEvent
    {
        public CreateUserEvent(User user) => User = user;

        public User User { get; set; }
    }
}
