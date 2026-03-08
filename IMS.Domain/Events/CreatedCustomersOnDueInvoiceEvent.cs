using IMS.Domain.Common;
using IMS.Domain.Entities;

namespace IMS.Domain.Events
{
    public class CreatedCustomersOnDueInvoiceEvent : BaseEvent
    {
        public List<Customer> Customers { get; set; }

        public CreatedCustomersOnDueInvoiceEvent(List<Customer> customers) => Customers = customers;
    }
}
