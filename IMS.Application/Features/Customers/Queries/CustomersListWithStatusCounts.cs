using IMS.Application.Common.Models;

namespace IMS.Application.Features.Customers.Queries
{
    public class CustomersListWithStatusCounts : PaginatedList<CustomerDto>
    {
        public CustomersListWithStatusCounts(PaginatedList<CustomerDto> pList, int pageSize)
        : base(pList.Items, pList.TotalCount, pList.PageNumber, pageSize) { }

        public int PendingTotalCount { get; set; }
        public int ActiveTotalCount { get; set; }
        public int InactiveTotalCount { get; set; }
    }
}
