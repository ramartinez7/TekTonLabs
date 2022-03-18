using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.Controllers.ProductController;

namespace Tests.MockData
{
    public class ProductData
    {
        public List<Product> ProductList { get; set; } 
        public Product Product { get; set; }
        public ProductDto ProductDto { get; set; } = new ProductDto("Product 1", 2500);
        public int ID { get; set; }
        public int NOT_EXISTS_ID { get; set; }
        public ProductData()
        {
            ID = 1;
            NOT_EXISTS_ID = -1;


            ProductList = new List<Product>()
            {
                new Product() { Id = 1, Name = "Product 1"}
            };

            Product = new Product() { Id = 1, Name = "Product 1" };
        }
    }
}
