using Microsoft.EntityFrameworkCore;
using CoreXCrud.Models;

namespace CoreXCrud.Data
{
    /// <summary>
    /// Veritabanı ile etkileşimde bulunacak DbContext sınıfı.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-Many ilişki
            modelBuilder.Entity<OrderDetail>()
                .HasOne(o => o.Order)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(p => p.Product)
                .WithMany(od => od.OrderDetails)
                .HasForeignKey(p => p.ProductId);

            // 📌 Decimal Alanları Hassasiyetle Tanımlama
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasPrecision(18, 2);
        }
    }
}