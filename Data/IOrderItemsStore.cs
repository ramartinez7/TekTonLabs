using Models;

namespace Data
{
    public interface IOrderItemsStore
    {
        Task<(Order order, List<Product> items)> GetOrderItemsByOrderId(int id);
    }
}