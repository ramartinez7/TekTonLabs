using Data;
using Mediator.Commands.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers.Order
{
    internal class CreateCommandHandler : IRequestHandler<CreateCommand, Models.Order>
    {
        public IEntityStore<Models.Order, int> Store { get; }

        public CreateCommandHandler(IEntityStore<Models.Order, int> store)
        {
            Store = store;
        }

        public async Task<Models.Order> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            return await Store.CreateAsync(request.Order);
        }
    }
}
