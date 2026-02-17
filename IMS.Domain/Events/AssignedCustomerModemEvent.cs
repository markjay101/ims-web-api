using IMS.Domain.Common;
using IMS.Domain.Entities;

namespace IMS.Domain.Events
{
    public class AssignedCustomerModemEvent : BaseEvent
    {
        public AssignedCustomerModemEvent(Customer customer) => Customer = customer;
        public Customer Customer { get; set; }
    }
}
