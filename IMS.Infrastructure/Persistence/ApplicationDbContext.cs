using IMS.Application.Common.Interfaces;
using IMS.Application.Features.Dashboard.Queries;
using IMS.Domain.Common.Interfaces;
using IMS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IMS.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> context, IMediator mediator) : DbContext(context), IApplicationDbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var result = await base.SaveChangesAsync(ct);

            var entitiesWithEvents = ChangeTracker.Entries<IBaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();

                foreach (var domaineEvent in domainEvents)
                {
                    await mediator.Publish(domaineEvent, ct);
                }
            }

            return result;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Domain.Entities.Application> Applications => Set<Domain.Entities.Application>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<CustomerPlan> CustomerPlans => Set<CustomerPlan>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<InternetPlan> InternetPlans => Set<InternetPlan>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Modem> Modems => Set<Modem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<SuperAdminDashboardSummaryDto> SuperAdminDashboardSummary => Set<SuperAdminDashboardSummaryDto>();
        public DbSet<AdminDashboardSummaryDto> AdminDashboardSummary => Set<AdminDashboardSummaryDto>();
    }
}
