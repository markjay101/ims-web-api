using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Persistence.EntityConfiguration
{
    internal class CustomerPlanConfiguration : IEntityTypeConfiguration<CustomerPlan>
    {
        public void Configure(EntityTypeBuilder<CustomerPlan> builder)
        {
            builder.HasOne(cp => cp.InternetPlan)
                   .WithMany(ip => ip.CustomerPlans)
                   .HasForeignKey(cp => cp.InternetPlanId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
