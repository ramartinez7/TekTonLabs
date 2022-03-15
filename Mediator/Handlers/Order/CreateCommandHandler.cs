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
        public CreateCommandHandler(IEntityStore<Models.Order> store)
        {
            Store = store;
        }

        public IEntityStore<Models.Order> Store { get; }

        public async Task<Models.Order> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            return await Store.CreateAsync(request.Order);
        }
    }
}
