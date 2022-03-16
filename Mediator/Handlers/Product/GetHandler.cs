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
    public class GetHandler : IRequestHandler<GetQuery, IEnumerable<Models.Product>>
    {
        public IEntityStore<Models.Product> Store { get; }

        public GetHandler(IEntityStore<Models.Product> store)
        {
            Store = store;
        }

        public async Task<IEnumerable<Models.Product>> Handle(GetQuery request, CancellationToken cancellationToken)
        {
            return await Store.GetAsync();
        }
    }
}
