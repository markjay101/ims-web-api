using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Persistence.EntityConfiguration
{
    internal class ApplicationEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.Application>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Application> builder)
        {
            builder.Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.HasMany(a => a.Documents)
                .WithOne()
                .HasForeignKey(d => d.ApplicationId);

            builder.HasOne(a => a.InternetPlan)
                .WithMany()
                .HasForeignKey(a => a.InternetPlanId);
        }
    }
}
