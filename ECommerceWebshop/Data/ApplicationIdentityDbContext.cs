using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceWebshop.Models;

namespace ECommerceWebshop.Data
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        // DbSets voor Identity zijn al gedefinieerd in IdentityDbContext
        // We hoeven alleen onze eigen DbSets toe te voegen
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customize Identity table names (optioneel)
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("Users");
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRoles");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserTokens");
            });

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
                new Product { Id = 1, Name = "Laptop Pro 15\"", Description = "High-performance laptop met 16GB RAM en 512GB SSD", Price = 1299.99m, Stock = 10, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=300&h=200&fit=crop" },
                new Product { Id = 2, Name = "Smartphone X12", Description = "5G smartphone met triple camera", Price = 899.99m, Stock = 25, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=300&h=200&fit=crop" },
                new Product { Id = 3, Name = "Wireless Headphones", Description = "Noise cancelling bluetooth koptelefoon", Price = 249.99m, Stock = 30, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=300&h=200&fit=crop" },
                new Product { Id = 4, Name = "Smartwatch Sport", Description = "Waterproof fitness tracker", Price = 299.99m, Stock = 20, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=300&h=200&fit=crop" },
                new Product { Id = 5, Name = "Tablet Pro 11\"", Description = "Krachtige tablet met stylus pen", Price = 749.99m, Stock = 15, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?w=300&h=200&fit=crop" },
                new Product { Id = 6, Name = "Bluetooth Speaker", Description = "Waterbestendige draadloze speaker", Price = 129.99m, Stock = 40, CategoryId = 1, ImageUrl = "https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=300&h=200&fit=crop" },

                // Kleding 
                new Product { Id = 7, Name = "Designer Jeans", Description = "Slim fit denim jeans", Price = 89.99m, Stock = 50, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=300&h=200&fit=crop" },
                new Product { Id = 8, Name = "Cotton T-Shirt", Description = "100% katoen comfort shirt", Price = 24.99m, Stock = 100, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=300&h=200&fit=crop" },
                new Product { Id = 9, Name = "Winter Jacket", Description = "Waterdichte winterjas met capuchon", Price = 179.99m, Stock = 15, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1548883354-94bcfe321cbb?w=300&h=200&fit=crop" },
                new Product { Id = 10, Name = "Running Shoes", Description = "Lichtgewicht sportschoenen", Price = 119.99m, Stock = 35, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=300&h=200&fit=crop" },
                new Product { Id = 11, Name = "Zomerjurk Floral", Description = "Lichte zomerjurk met bloemenprint", Price = 59.99m, Stock = 25, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1572804013427-4d7ca7268217?w=300&h=200&fit=crop" },
                new Product { Id = 12, Name = "Business Overhemd", Description = "Formeel overhemd 100% katoen", Price = 49.99m, Stock = 45, CategoryId = 2, ImageUrl = "https://images.unsplash.com/photo-1596755094514-f87e34085b2c?w=300&h=200&fit=crop" },

                // Boeken 
                new Product { Id = 13, Name = "De Beste Thriller 2024", Description = "Spannende bestseller van dit jaar", Price = 22.50m, Stock = 60, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=300&h=200&fit=crop" },
                new Product { Id = 14, Name = "Kookboek Wereldkeuken", Description = "500 recepten uit alle werelddelen", Price = 35.99m, Stock = 25, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1490645935967-10de6ba17061?w=300&h=200&fit=crop" },
                new Product { Id = 15, Name = "Programmeren voor Beginners", Description = "Leer programmeren met C#", Price = 45.00m, Stock = 20, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1515879218367-8466d910aaa4?w=300&h=200&fit=crop" },
                new Product { Id = 16, Name = "Historische Roman", Description = "Meeslepend verhaal uit de middeleeuwen", Price = 19.99m, Stock = 35, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1524995997946-a1c2e315a42f?w=300&h=200&fit=crop" },
                new Product { Id = 17, Name = "Kinderverhalen Bundel", Description = "20 klassieke sprookjes voor het slapengaan", Price = 15.99m, Stock = 50, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1512820790803-83ca734da794?w=300&h=200&fit=crop" },
                new Product { Id = 18, Name = "Reisguide Europa", Description = "Complete gids voor 30 Europese steden", Price = 29.99m, Stock = 30, CategoryId = 3, ImageUrl = "https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=300&h=200&fit=crop" },

                // Sport
                new Product { Id = 19, Name = "Yoga Mat Pro", Description = "Anti-slip yoga mat 6mm", Price = 39.99m, Stock = 40, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1601925260368-ae2f83cf8b7f?w=300&h=200&fit=crop" },
                new Product { Id = 20, Name = "Dumbbell Set 20kg", Description = "Verstelbare halters set", Price = 149.99m, Stock = 12, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=300&h=200&fit=crop" },
                new Product { Id = 21, Name = "Voetbal Official", Description = "Professionele wedstrijdbal", Price = 59.99m, Stock = 30, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1551958219-acbc608c6377?w=300&h=200&fit=crop" },
                new Product { Id = 34, Name = "Tennis Racket Pro", Description = "Professioneel tennis racket met tas", Price = 189.99m, Stock = 15, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1622279457486-62dcc4a431d6?w=300&h=200&fit=crop" },
                new Product { Id = 35, Name = "Fiets Set", Description = "Set van 6 fietsen", Price = 29.99m, Stock = 50, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1598289431512-b97b0917affc?w=300&h=200&fit=crop" },
                new Product { Id = 36, Name = "Sporttas Deluxe", Description = "Ruime sporttas met schoenenvak", Price = 54.99m, Stock = 25, CategoryId = 4, ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=300&h=200&fit=crop" },

                // Huis & Tuin
                new Product { Id = 22, Name = "Robot Stofzuiger", Description = "Smart home stofzuiger met app control", Price = 399.99m, Stock = 8, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=300&h=200&fit=crop" },
                new Product { Id = 23, Name = "LED Bureaulamp", Description = "Dimbare bureaulamp met USB", Price = 49.99m, Stock = 25, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=300&h=200&fit=crop" },
                new Product { Id = 24, Name = "Tuinset 4 Personen", Description = "Weerbestendige tuinmeubelen", Price = 699.99m, Stock = 5, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1506439773649-6e0eb8cfb237?w=300&h=200&fit=crop" },
                new Product { Id = 25, Name = "Koffiemachine Deluxe", Description = "Volautomatische espressomachine", Price = 899.99m, Stock = 10, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1514432324607-a09d9b4aefdd?w=300&h=200&fit=crop" },
                new Product { Id = 26, Name = "Smart Thermostaat", Description = "WiFi thermostaat met app bediening", Price = 249.99m, Stock = 18, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1585421514284-efb74c2b69ba?w=300&h=200&fit=crop" },
                new Product { Id = 27, Name = "Decoratieve Plantenpot Set", Description = "Set van 3 moderne plantenpotten", Price = 89.99m, Stock = 22, CategoryId = 5, ImageUrl = "https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=300&h=200&fit=crop" },

                // Speelgoed 
                new Product { Id = 28, Name = "LEGO Creator Set", Description = "3-in-1 bouwset 500+ stukjes", Price = 79.99m, Stock = 20, CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1587654780291-39c9404d746b?w=300&h=200&fit=crop" },
                new Product { Id = 29, Name = "RC Racing Car", Description = "Bestuurbare auto 1:10 schaal", Price = 129.99m, Stock = 15, CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1594736797933-d0501ba2fe65?w=300&h=200&fit=crop" },
                new Product { Id = 30, Name = "Bordspel Avontuur", Description = "Strategisch familiespel 2-6 spelers", Price = 44.99m, Stock = 30, CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1611891487122-207579d67d98?w=300&h=200&fit=crop" },
                new Product { Id = 31, Name = "Houten Blokkenset", Description = "Educatief speelgoed 100 blokken", Price = 34.99m, Stock = 25, CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1558618666-fcd25c85cd64?w=300&h=200&fit=crop" },
                new Product { Id = 32, Name = "Pop met Accessoires", Description = "Fashion pop met 20 kledingsets", Price = 49.99m, Stock = 35, CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1603362305486-75a25f73051a?w=300&h=200&fit=crop" },
                new Product { Id = 33, Name = "Puzzel 1000 Stukjes", Description = "Natuurlandschap puzzel voor volwassenen", Price = 24.99m, Stock = 40, CategoryId = 6, ImageUrl = "https://images.unsplash.com/photo-1494059980473-813e73ee784b?w=300&h=200&fit=crop" }
            );
        }
    }
}