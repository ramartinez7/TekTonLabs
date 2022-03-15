using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class OrderItemsStore : IOrderItemsStore
    {
        private readonly IEntityStore<Order> OrderStore;
        private readonly IEntityStore<OrderItems> _OrderItemsStore;

        public OrderItemsStore(IEntityStore<Order> orderStore, IEntityStore<OrderItems> orderItemsStore)
        {
            OrderStore = orderStore;
            _OrderItemsStore = orderItemsStore;
        }

        public async Task<(Order order, List<Product> items)> GetOrderItemsByOrderId(int id)
        {
            Order order = await OrderStore.EntitySet()
                .Where(o => o.Id == id)
                .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync();

            if (order is null)
            {
                throw new InvalidOperationException($"Order {id} not found");
            }

            List<Product> items = order.Items.Select(o => o.Product).ToList();

            return (order, items);
        }
    }
}
