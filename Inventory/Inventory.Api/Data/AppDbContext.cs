using InventoryApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Indexes
            modelBuilder.Entity<Product>().HasIndex(p => p.CategoryId); // FK Index
            modelBuilder.Entity<Product>().HasIndex(p => p.IsActive);   // Filter Index
            
            // Seeding Data
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Gadgets", IsActive = true },
                new Category { Id = 2, Name = "Books", Description = "Readables", IsActive = true },
                new Category { Id = 3, Name = "Clothing", Description = "Wearables", IsActive = true },
                new Category { Id = 4, Name = "Home", Description = "Furniture", IsActive = true }
            };
            modelBuilder.Entity<Category>().HasData(categories);

            var products = new List<Product>{
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 1200.00m, StockQuantity = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 799.99m, StockQuantity = 25, CategoryId = 1 },
                new Product { Id = 3, Name = "C# in Depth", Description = "A programming book", Price = 49.99m, StockQuantity = 50, CategoryId = 2 },
                new Product { Id = 4, Name = "T-Shirt", Description = "Cotton T-Shirt", Price = 19.99m, StockQuantity = 100, CategoryId = 3 },
                new Product { Id = 5, Name = "Coffee Maker", Description = "Drip coffee maker", Price = 89.50m, StockQuantity = 30, CategoryId = 4 },
                new Product { Id = 6, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse", Price = 35.00m, StockQuantity = 0, CategoryId = 1, IsActive = true }, // Out of stock
                new Product { Id = 7, Name = "The Great Gatsby", Description = "A classic novel", Price = 12.99m, StockQuantity = 75, CategoryId = 2 },
                new Product { Id = 8, Name = "Jeans", Description = "Denim jeans", Price = 55.00m, StockQuantity = 40, CategoryId = 3 },
                new Product { Id = 9, Name = "Blender", Description = "High-speed blender", Price = 120.00m, StockQuantity = 15, CategoryId = 4 },
                new Product { Id = 10, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 249.99m, StockQuantity = 20, CategoryId = 1 },
                new Product { Id = 11, Name = "Keyboard", Description = "Mechanical keyboard", Price = 150.00m, StockQuantity = 5, CategoryId = 1 },
                new Product { Id = 12, Name = "Domain-Driven Design", Description = "A software design book", Price = 65.00m, StockQuantity = 10, CategoryId = 2 },
                new Product { Id = 13, Name = "Hoodie", Description = "Warm fleece hoodie", Price = 45.00m, StockQuantity = 60, CategoryId = 3 },
                new Product { Id = 14, Name = "Toaster", Description = "4-slice toaster", Price = 40.00m, StockQuantity = 22, CategoryId = 4 },
                new Product { Id = 15, Name = "Old Product", Description = "Discontinued item", Price = 10.00m, StockQuantity = 0, CategoryId = 1, IsActive = false } // Inactive
            };      
            modelBuilder.Entity<Category>().HasData(products);      
        }
    }
}