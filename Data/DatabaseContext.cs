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
        public virtual DbSet<OrderItems> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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

            builder.Entity<OrderItems>().HasData(new List<OrderItems>()
            {
                new OrderItems()
                {
                    Id = 1, 
                    OrderId = 1,
                    ProductId = 1,
                    ProductQuantity = 23
                },
                new OrderItems()
                {
                    Id = 2,
                    OrderId = 1,
                    ProductId = 2,
                    ProductQuantity = 14
                }
            });
        }
    }
}
