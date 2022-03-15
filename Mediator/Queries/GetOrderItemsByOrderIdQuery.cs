using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Queries
{
    public record GetOrderItemsByOrderIdQuery(int Id) : IRequest<(Order order, List<Product> items)>;
}
