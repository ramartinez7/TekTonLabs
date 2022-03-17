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
    public class DeleteOrderItemCommandHandler : IRequestHandler<Mediator.Commands.OrderItem.DeleteOrderItemCommand, bool>
    {
        public IEntityStore<Models.OrderItem, int> Store { get; }

        public DeleteOrderItemCommandHandler(IEntityStore<Models.OrderItem, int> store)
        {
            Store = store;
        }


        public Task<bool> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var result = Store.DeleteAsync(request.Id);
            return Task.FromResult(result);
        }
    }
}
