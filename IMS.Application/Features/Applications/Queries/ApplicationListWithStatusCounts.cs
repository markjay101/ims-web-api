using IMS.Application.Common.Models;

namespace IMS.Application.Features.Applications.Queries
{
    public class ApplicationListWithStatusCounts : PaginatedList<ApplicationDto>
    {
        public ApplicationListWithStatusCounts(PaginatedList<ApplicationDto> pList, int pageSize) 
            : base(pList.Items, pList.TotalCount, pList.PageNumber, pageSize){}
        public int PendingTotalCount { get; set; }
        public int ApprovedTotalCount { get; set; }
        public int RejectedTotalCount { get; set; }
    }
}
