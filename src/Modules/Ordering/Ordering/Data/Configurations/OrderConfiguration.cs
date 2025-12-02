using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.OrderName)
                   .IsUnique();
            builder.Property(o => o.OrderName)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.Property(o => o.CustomerId);

            builder.HasMany(o => o.OrderItems)
                   .WithOne()
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(200);
                addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(200);
                addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);
                addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(200);
                addressBuilder.Property(a => a.State).HasMaxLength(100);
                addressBuilder.Property(a => a.Country).IsRequired().HasMaxLength(100);
                addressBuilder.Property(a => a.ZipCode).IsRequired().HasMaxLength(5);
            });
            builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FirstName).IsRequired().HasMaxLength(200);
                addressBuilder.Property(a => a.LastName).IsRequired().HasMaxLength(200);
                addressBuilder.Property(a => a.EmailAddress).HasMaxLength(50);
                addressBuilder.Property(a => a.AddressLine).IsRequired().HasMaxLength(200);
                addressBuilder.Property(a => a.State).HasMaxLength(100);
                addressBuilder.Property(a => a.Country).IsRequired().HasMaxLength(100);
                addressBuilder.Property(a => a.ZipCode).IsRequired().HasMaxLength(5);
            });
            builder.ComplexProperty(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(p => p.CardName).IsRequired().HasMaxLength(50);
                paymentBuilder.Property(p => p.CardNumber).IsRequired().HasMaxLength(24);
                paymentBuilder.Property(p => p.Expiration).IsRequired().HasMaxLength(10);
                paymentBuilder.Property(p => p.CVV).IsRequired().HasMaxLength(3);
                paymentBuilder.Property(p => p.PaymentMethod);
            });
        }
    }
}
