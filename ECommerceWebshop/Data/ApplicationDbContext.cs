using Microsoft.EntityFrameworkCore;
using ECommerceWebshop.Models;

namespace ECommerceWebshop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Elektronica", Description = "Computers, smartphones, tablets en meer", Icon = "📱" },
                new Category { Id = 2, Name = "Kleding", Description = "Mode voor mannen, vrouwen en kinderen", Icon = "👕" },
                new Category { Id = 3, Name = "Boeken", Description = "Fictie, non-fictie, educatief", Icon = "📚" },
                new Category { Id = 4, Name = "Sport", Description = "Sportartikelen en fitness", Icon = "⚽" },
                new Category { Id = 5, Name = "Huis & Tuin", Description = "Alles voor huis en tuin", Icon = "🏠" },
                new Category { Id = 6, Name = "Speelgoed", Description = "Speelgoed voor alle leeftijden", Icon = "🎮" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                // Elektronica
                new Product { Id = 1, Name = "Laptop Pro 15\"", Description = "High-performance laptop met 16GB RAM en 512GB SSD", Price = 1299.99m, Stock = 10, CategoryId = 1, ImageUrl = "https://via.placeholder.com/300x200?text=Laptop+Pro" },
                new Product { Id = 2, Name = "Smartphone X12", Description = "5G smartphone met triple camera", Price = 899.99m, Stock = 25, CategoryId = 1, ImageUrl = "https://via.placeholder.com/300x200?text=Smartphone" },
                new Product { Id = 3, Name = "Wireless Headphones", Description = "Noise cancelling bluetooth koptelefoon", Price = 249.99m, Stock = 30, CategoryId = 1, ImageUrl = "https://via.placeholder.com/300x200?text=Headphones" },
                new Product { Id = 4, Name = "Smartwatch Sport", Description = "Waterproof fitness tracker", Price = 299.99m, Stock = 20, CategoryId = 1, ImageUrl = "https://via.placeholder.com/300x200?text=Smartwatch" },

                // Kleding
                new Product { Id = 5, Name = "Designer Jeans", Description = "Slim fit denim jeans", Price = 89.99m, Stock = 50, CategoryId = 2, ImageUrl = "https://via.placeholder.com/300x200?text=Jeans" },
                new Product { Id = 6, Name = "Cotton T-Shirt", Description = "100% katoen comfort shirt", Price = 24.99m, Stock = 100, CategoryId = 2, ImageUrl = "https://via.placeholder.com/300x200?text=T-Shirt" },
                new Product { Id = 7, Name = "Winter Jacket", Description = "Waterdichte winterjas met capuchon", Price = 179.99m, Stock = 15, CategoryId = 2, ImageUrl = "https://via.placeholder.com/300x200?text=Winter+Jacket" },
                new Product { Id = 8, Name = "Running Shoes", Description = "Lichtgewicht sportschoenen", Price = 119.99m, Stock = 35, CategoryId = 2, ImageUrl = "https://via.placeholder.com/300x200?text=Running+Shoes" },

                // Boeken
                new Product { Id = 9, Name = "De Beste Thriller 2024", Description = "Spannende bestseller van dit jaar", Price = 22.50m, Stock = 60, CategoryId = 3, ImageUrl = "https://via.placeholder.com/300x200?text=Thriller+Book" },
                new Product { Id = 10, Name = "Kookboek Wereldkeuken", Description = "500 recepten uit alle werelddelen", Price = 35.99m, Stock = 25, CategoryId = 3, ImageUrl = "https://via.placeholder.com/300x200?text=Cookbook" },
                new Product { Id = 11, Name = "Programmeren voor Beginners", Description = "Leer programmeren met C#", Price = 45.00m, Stock = 20, CategoryId = 3, ImageUrl = "https://via.placeholder.com/300x200?text=Programming+Book" },

                // Sport
                new Product { Id = 12, Name = "Yoga Mat Pro", Description = "Anti-slip yoga mat 6mm", Price = 39.99m, Stock = 40, CategoryId = 4, ImageUrl = "https://via.placeholder.com/300x200?text=Yoga+Mat" },
                new Product { Id = 13, Name = "Dumbbell Set 20kg", Description = "Verstelbare halters set", Price = 149.99m, Stock = 12, CategoryId = 4, ImageUrl = "https://via.placeholder.com/300x200?text=Dumbbells" },
                new Product { Id = 14, Name = "Voetbal Official", Description = "Professionele wedstrijdbal", Price = 59.99m, Stock = 30, CategoryId = 4, ImageUrl = "https://via.placeholder.com/300x200?text=Football" },

                // Huis & Tuin
                new Product { Id = 15, Name = "Robot Stofzuiger", Description = "Smart home stofzuiger met app control", Price = 399.99m, Stock = 8, CategoryId = 5, ImageUrl = "https://via.placeholder.com/300x200?text=Robot+Vacuum" },
                new Product { Id = 16, Name = "LED Bureaulamp", Description = "Dimbare bureaulamp met USB", Price = 49.99m, Stock = 25, CategoryId = 5, ImageUrl = "https://via.placeholder.com/300x200?text=Desk+Lamp" },
                new Product { Id = 17, Name = "Tuinset 4 Personen", Description = "Weerbestendige tuinmeubelen", Price = 699.99m, Stock = 5, CategoryId = 5, ImageUrl = "https://via.placeholder.com/300x200?text=Garden+Set" },

                // Speelgoed
                new Product { Id = 18, Name = "LEGO Creator Set", Description = "3-in-1 bouwset 500+ stukjes", Price = 79.99m, Stock = 20, CategoryId = 6, ImageUrl = "https://via.placeholder.com/300x200?text=LEGO+Set" },
                new Product { Id = 19, Name = "RC Racing Car", Description = "Bestuurbare auto 1:10 schaal", Price = 129.99m, Stock = 15, CategoryId = 6, ImageUrl = "https://via.placeholder.com/300x200?text=RC+Car" },
                new Product { Id = 20, Name = "Bordspel Avontuur", Description = "Strategisch familiespel 2-6 spelers", Price = 44.99m, Stock = 30, CategoryId = 6, ImageUrl = "https://via.placeholder.com/300x200?text=Board+Game" }
            );
        }
    }
}