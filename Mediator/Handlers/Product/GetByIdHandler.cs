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
    public class GetByIdHandler : IRequestHandler<Mediator.Queries.Product.GetByIdQuery, Models.Product>
    {
        public IEntityStore<Models.Product, int> Store { get; }

        public GetByIdHandler(IEntityStore<Models.Product, int> store)
        {
            Store = store;
        }

        public async Task<Models.Product> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            return await Store.GetByIdAsync(request.Id);
        }
    }
}
