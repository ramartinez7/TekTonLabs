using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Queries.OrderItem
{
    public record ExistsQuery(int Id) : IRequest<bool> { }
}
