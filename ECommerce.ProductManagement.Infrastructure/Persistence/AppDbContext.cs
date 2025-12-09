using ECommerce.ProductManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .HasIndex(p => p.Sku)
                .IsUnique();

            builder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}
