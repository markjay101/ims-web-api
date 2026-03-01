using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Persistence.EntityConfiguration
{
    internal class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Status)
                   .HasConversion<string>()
                   .HasMaxLength(10);

            builder.HasOne(c => c.Application)
                .WithOne()
                .HasForeignKey<Customer>(c => c.ApplicationId);

            builder.HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Customer>(c => c.UserId);

            builder.HasOne(c => c.Plan)
                .WithOne()
                .HasForeignKey<CustomerPlan>(cp => cp.CustomerId);

            builder.HasOne(c => c.Modem)
                .WithOne(m => m.Customer)
                .HasForeignKey<Customer>(c => c.ModemId);

            builder.HasMany(c => c.Invoices)
                .WithOne()
                .HasForeignKey(i => i.CustomerId);
        }
    }
}
