using Data;
using Mediator.Commands.OrderItem;
using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers.OrderItem
{
    public class AddOrderItemCommandHandler : IRequestHandler<Mediator.Commands.OrderItem.AddOrderItemCommand, Models.OrderItem>
    {
        public IEntityStore<Models.OrderItem, int> Store { get; }

        public AddOrderItemCommandHandler(IEntityStore<Models.OrderItem, int> store)
        {
            Store = store;
        }

        public async Task<Models.OrderItem> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            return await Store.CreateAsync(request.OrderItem);
        }
    }
}
