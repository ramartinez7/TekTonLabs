using Data;
using Mediator.Queries.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers.Product
{
    public class ExistsQueryHandler : IRequestHandler<Mediator.Queries.Product.ExistsQuery, bool>
    {
        public IEntityStore<Models.Product, int> Store { get; }

        public ExistsQueryHandler(IEntityStore<Models.Product, int> store)
        {
            Store = store;
        }

        public async Task<bool> Handle(ExistsQuery request, CancellationToken cancellationToken)
        {
            return await Store.ExistsAsync(request.Id);
        }
    }
}
