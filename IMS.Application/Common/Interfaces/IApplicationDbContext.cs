using IMS.Application.Features.Dashboard.Queries;
using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        DbSet<User> Users { get; }
        DbSet<Domain.Entities.Application> Applications { get; }
        DbSet<Customer> Customers{ get; }
        DbSet<CustomerPlan> CustomerPlans { get; }
        DbSet<Document> Documents { get; }
        DbSet<InternetPlan> InternetPlans { get; }
        DbSet<Invoice> Invoices { get; }
        DbSet<Modem> Modems { get; }
        DbSet<Payment> Payments { get; }
        DbSet<PaymentMethod> PaymentMethods { get; }
        DbSet<SuperAdminDashboardSummaryDto> SuperAdminDashboardSummary { get; }
        DbSet<AdminDashboardSummaryDto> AdminDashboardSummary { get; }
    }
}
