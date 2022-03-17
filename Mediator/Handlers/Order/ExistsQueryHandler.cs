using Data;
using Mediator.Queries.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers.Order
{
    public class ExistsQueryHandler : IRequestHandler<Mediator.Queries.Order.ExistsQuery, bool>
    {
        public IEntityStore<Models.Order, int> Store { get; }

        public ExistsQueryHandler(IEntityStore<Models.Order, int> store)
        {
            Store = store;
        }

        public async Task<bool> Handle(ExistsQuery request, CancellationToken cancellationToken)
        {
            return await Store.ExistsAsync(request.Id);
        }
    }
}
