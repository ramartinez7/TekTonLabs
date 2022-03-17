using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class OrderItem : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        [Required]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        [Required]
        [MinLength(1)]
        public double ProductQuantity { get; set; }
    }
}
