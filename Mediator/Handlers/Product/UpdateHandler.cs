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
    public class UpdateHandler : IRequestHandler<UpdateCommand, Models.Product>
    {
        public IEntityStore<Models.Product> Store { get; }

        public UpdateHandler(IEntityStore<Models.Product> store)
        {
            Store = store;
        }

        public async Task<Models.Product> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            return await Store.UpdateAsync(request.Product);
        }
    }
}
