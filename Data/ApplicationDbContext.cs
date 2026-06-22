using Microsoft.EntityFrameworkCore;
using ProductManagement.Models;

namespace ProductManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop Pro 15",
                    Description = "High-performance laptop with 16GB RAM and 512GB SSD",
                    Price = 1299.99m,
                    Stock = 25,
                    Category = "Electronics",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Wireless Mouse",
                    Description = "Ergonomic wireless mouse with 2.4GHz connectivity",
                    Price = 29.99m,
                    Stock = 150,
                    Category = "Accessories",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                },
                new Product
                {
                    Id = 3,
                    Name = "USB-C Hub",
                    Description = "7-in-1 USB-C hub with HDMI, USB 3.0, and SD card slots",
                    Price = 49.99m,
                    Stock = 80,
                    Category = "Accessories",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsActive = true
                }
            );
        }
    }
}
