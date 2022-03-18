using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductAdditionalData
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("currentStock")]
        public int CurrentStock { get; set; }
        public string Message { get; set; }

        public static ProductAdditionalData GetErrorHandler(Exception ex)
        {
            return new ProductAdditionalData
            {
                Id = -1,
                CurrentStock = -1,
                Message = $"Something ocurred connecting to external API: {ex.Message}"
            };
        }
    }
}
