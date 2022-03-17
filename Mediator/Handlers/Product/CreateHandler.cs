using Data;
using Mediator.Commands.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Handlers.Product
{
    public class CreateHandler : IRequestHandler<Mediator.Commands.Product.CreateCommand, Models.Product>
    {
        public IEntityStore<Models.Product, int> Store { get; }

        public CreateHandler(IEntityStore<Models.Product, int> store)
        {
            Store = store;
        }

        public async Task<Models.Product> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            return await Store.CreateAsync(request.Product);
        }
    }
}
