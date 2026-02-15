namespace IMS.Application.Features.Dashboard.Queries
{
    public abstract class BaseDashboardSummaryDto
    {
        public int UnpaidInvoicesCount { get; set; }
        public int PendingCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int InactiveCustomers { get; set; }
        public int PendingApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public int AvailableModems { get; set; }
        public int AssignedModems { get; set; }
    }

    public class SuperAdminDashboardSummaryDto : BaseDashboardSummaryDto
    {
        public decimal Earnings { get; set; }
        public int AdminAccounts { get; set; }
        public decimal UnpaidInvoicesTotalAmount { get; set; }
    }

    public class AdminDashboardSummaryDto : BaseDashboardSummaryDto {}
}
