namespace Basket.Data.Configurations
{
    public class ShopingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.UserName)
                   .IsUnique();

            builder.Property(c => c.UserName)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.HasMany(c => c.Items)
                   .WithOne()
                   .HasForeignKey(i => i.ShoppingCartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
