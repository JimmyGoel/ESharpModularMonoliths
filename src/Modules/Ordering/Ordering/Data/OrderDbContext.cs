using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Ordering.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> Options) : base(Options)
        {
        }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItem => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("ordering");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
