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
    public class GetHandler : IRequestHandler<GetQuery, IEnumerable<Models.Order>>
    {
        public IEntityStore<Models.Order> Store { get; }

        public GetHandler(IEntityStore<Models.Order> store)
        {
            Store = store;
        }

        public async Task<IEnumerable<Models.Order>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            return await Store.GetAsync();
        }
    }
}
