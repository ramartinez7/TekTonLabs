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
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Models.Order>
    {
        public IEntityStore<Models.Order> Store { get; }

        public GetByIdQueryHandler(IEntityStore<Models.Order> store)
        {
            Store = store;
        }

        public async Task<Models.Order> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await Store.GetByIdAsync(request.Id);
        }
    }
}
