using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderItem>()
                .HasIndex(p => new { p.OrderId, p.ProductId }).IsUnique(); // This combination should be unique // InMemory will allow you to save data that would violate referential integrity constraints in a relational database. Taken from: https://stackoverflow.com/questions/52259580/adding-a-unique-index-on-ef-core-mapping-does-not-seem-to-work

            builder.Entity<Order>().HasData(new List<Order>()
            {
                new Order()
                {
                    Id = 1,
                    Date = DateTime.Now
                },
                new Order()
                {
                    Id = 2,
                    Date = DateTime.Now
                },
                new Order()
                {
                    Id = 3,
                    Date = DateTime.Now
                }
            });

            builder.Entity<Product>().HasData(new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Name = "Product 1",
                    Price = 2000.23
                },
                new Product()
                {
                    Id = 2,
                    Name =  "Product 2",
                    Price = 3102
                },
                new Product()
                {
                    Id = 3,
                    Name = "Product 3",
                    Price = 2900
                }
            });

            builder.Entity<OrderItem>().HasData(new List<OrderItem>()
            {
                new OrderItem()
                {
                    Id = 1, 
                    OrderId = 1,
                    ProductId = 1,
                    ProductQuantity = 23
                },
                new OrderItem()
                {
                    Id = 2,
                    OrderId = 2,
                    ProductId = 1,
                    ProductQuantity = 14
                }
            });
        }
    }
}
