using IMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.Persistence.EntityConfiguration
{
    internal class InvoiceEntityConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.Property(i => i.Status)
                   .HasConversion<string>()
                   .HasMaxLength(10);

            builder.HasOne(i => i.Payment)
                .WithOne()
                .HasForeignKey<Payment>(p => p.InvoiceId);
        }
    }
}
