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
    public class UpdateCommandHandler : IRequestHandler<UpdateCommand, Models.Order>
    {
        public IEntityStore<Models.Order> Store { get; }

        public UpdateCommandHandler(IEntityStore<Models.Order> store)
        {
            Store = store;
        }

        public async Task<Models.Order> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            return await Store.UpdateAsync(request.Order);
        }
    }
}
