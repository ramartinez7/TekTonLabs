using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Queries.Product
{
    public record GetQuery() : IRequest<IEnumerable<Models.Product>> { }
}
