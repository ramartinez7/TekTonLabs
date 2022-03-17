using Data;
using Mediator.Queries.OrderItem;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers.OrderItem
{
    public class ExistsQueryHandler : IRequestHandler<Mediator.Queries.OrderItem.ExistsQuery, bool>
    {
        public IEntityStore<Models.OrderItem, int> Store { get; }

        public ExistsQueryHandler(IEntityStore<Models.OrderItem, int> store)
        {
            Store = store;
        }

        public async Task<bool> Handle(ExistsQuery request, CancellationToken cancellationToken)
        {
            return await Store.ExistsAsync(request.Id);
        }
    }
}
