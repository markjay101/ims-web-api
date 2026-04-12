using IMS.Domain.Common;
using IMS.Domain.Entities;

namespace IMS.Domain.Events
{
    public class CreatedApplicationEvent : BaseEvent
    {
        public CreatedApplicationEvent(Application application) => Application = application;
        public Application Application { get; }
    }
}
