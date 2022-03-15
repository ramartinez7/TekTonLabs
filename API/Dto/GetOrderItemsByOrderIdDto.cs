using Models;

namespace API.Dto
{
    public class GetOrderItemsByOrderIdDto
    {
        public Order Order { get; set; }
        public List<Product> Items { get; set; }
    }
}
