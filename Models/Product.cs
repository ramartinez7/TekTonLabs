using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Product : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(256)]
        [MinLength(1)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        public double Price { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProductAdditionalData AdditionalData { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem> Items { get; set; }

    }
}
