using Data;
using Mediator.Queries;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers
{
    public class GetOrderItemsByOrderIdHandler : IRequestHandler<GetOrderItemsByOrderIdQuery, (Order order, List<Product> items)>
    {
        public IOrderItemsStore Store { get; }

        public GetOrderItemsByOrderIdHandler(IOrderItemsStore store)
        {
            Store = store;
        }

        public Task<(Order order, List<Product> items)> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
        {
            return Store.GetOrderItemsByOrderId(request.Id);
        }
    }
}
