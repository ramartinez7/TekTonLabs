using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Order : IEntity<int>
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderAdditionalData AdditionalData { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem> Items { get; set; }
    }

    public class OrderAdditionalData
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
    }
}
