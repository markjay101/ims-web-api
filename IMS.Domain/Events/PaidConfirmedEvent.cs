using IMS.Domain.Common;
using IMS.Domain.Entities;

namespace IMS.Domain.Events
{
    public class PaidConfirmedEvent : BaseEvent
    {
        public Invoice Invoice { get; set; }

        public PaidConfirmedEvent(Invoice invoice) => Invoice = invoice;
    }
}
