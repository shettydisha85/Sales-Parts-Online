using Microsoft.EntityFrameworkCore;
using SalesPartsOnline.Models;

namespace SalesPartsOnline.DAL
{
    public class SPODbContext :DbContext
    {
        public SPODbContext(DbContextOptions<SPODbContext> options) :base(options)
        { 
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasKey(b => b.productId);
            modelBuilder.Entity<Product>().Property(b => b.productId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(b => b.description).IsRequired();
            modelBuilder.Entity<Product>().Property(b => b.stock).IsRequired();
            modelBuilder.Entity<Product>().Property(b => b.imagesURL).IsRequired();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Configure other relationships as needed
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ProductId });

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);


        }

        public void SeedData()
        {
        //    SeedCategories();
        //    SeedProducts();
        //    SeedUsers();
        //    SeedOrders();
        }

        private void SeedCategories()
        {
            if (!Categories.Any())
            {
                var categories = new List<Category>
        {
            new Category { CategoryId = Guid.NewGuid(), Name = "Electronics", Description = "Electronic devices and gadgets" },
            new Category { CategoryId = Guid.NewGuid(), Name = "Automotive Parts", Description = "Parts and accessories for automobiles" },
            new Category { CategoryId = Guid.NewGuid(), Name = "Clothing", Description = "Apparel and garments" },
            new Category { CategoryId = Guid.NewGuid(), Name = "Home Appliances", Description = "Appliances for household use" },
            new Category { CategoryId = Guid.NewGuid(), Name = "Furniture", Description = "Furniture and furnishings" }
        };
                Categories.AddRange(categories);
                SaveChanges();
            }
        }

        private void SeedProducts()
        {
            if (!Products.Any())
            {
                var categories = Categories.ToList();
                var products = new List<Product>();

                foreach (var category in categories)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        products.Add(new Product
                        {
                            productId = Guid.NewGuid(),
                            name = GetProductName(i, category.Name),
                            CategoryId = category.CategoryId, // Assign the CategoryId instead of Category object
                            price = GetProductPrice(i)
                        });
                    }
                }

                Products.AddRange(products);
                SaveChanges();
            }
        }

        private string GetProductName(int index, string category)
        {
            switch (category)
            {
                case "Electronics":
                    return $"Smartphone {index}";
                case "Automotive Parts":
                    return $"Engine Oil {index}";
                case "Clothing":
                    return $"T-shirt {index}";
                case "Home Appliances":
                    return $"Microwave {index}";
                case "Furniture":
                    return $"Sofa {index}";
                default:
                    return $"Product {index}";
            }
        }

        private decimal GetProductPrice(int index)
        {
            switch (index)
            {
                case 1: return 500.00m;
                case 2: return 25.00m;
                case 3: return 15.00m;
                case 4: return 200.00m;
                case 5: return 750.00m;
                default: return 0.00m;
            }
        }

        private void SeedUsers()
        {
            if (!Users.Any())
            {
                var users = new List<User>
        {
            new User { UserId = Guid.NewGuid(), Username = "john_doe", Email = "john@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "jane_smith", Email = "jane@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "mike_jones", Email = "mike@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "sarah_davis", Email = "sarah@example.com" },
            new User { UserId = Guid.NewGuid(), Username = "alex_wilson", Email = "alex@example.com" }
        };
                Users.AddRange(users);
                SaveChanges();
            }
        }

        private void SeedOrders()
        {
            if (!Orders.Any())
            {
                var users = Users.ToList();
                var orders = new List<Order>();

                foreach (var user in users)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        orders.Add(new Order
                        {
                            OrderId = Guid.NewGuid(),
                            UserId = user.UserId,
                            OrderDate = DateTime.Now.AddDays(-i),
                            TotalAmount = GetTotalAmount(i)
                        });
                    }
                }

                Orders.AddRange(orders);
                SaveChanges();
            }
        }

        private decimal GetTotalAmount(int index)
        {
            switch (index)
            {
                case 1: return 100.00m;
                case 2: return 50.00m;
                case 3: return 75.00m;
                case 4: return 200.00m;
                case 5: return 150.00m;
                default: return 0.00m;
            }
        }

    }
}
