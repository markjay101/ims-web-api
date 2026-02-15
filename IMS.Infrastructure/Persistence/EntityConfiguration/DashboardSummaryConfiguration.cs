using IMS.Application.Features.Dashboard.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Persistence.EntityConfiguration
{
    public class SuperAdminDashboardSummaryConfiguration : IEntityTypeConfiguration<SuperAdminDashboardSummaryDto>
    {
        public void Configure(EntityTypeBuilder<SuperAdminDashboardSummaryDto> builder)
        {
            builder.HasNoKey().ToView(null);
            builder.Property(e => e.Earnings).HasPrecision(18, 2);
            builder.Property(e => e.UnpaidInvoicesTotalAmount).HasPrecision(18, 2);
        }
    }

    public class AdminDashboardSummaryConfiguration : IEntityTypeConfiguration<AdminDashboardSummaryDto>
    {
        public void Configure(EntityTypeBuilder<AdminDashboardSummaryDto> builder)
        {
            builder.HasNoKey().ToView(null);
        }
    }
}
