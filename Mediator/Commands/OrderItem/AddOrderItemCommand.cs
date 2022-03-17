using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Commands.OrderItem
{
    public record AddOrderItemCommand(int OrderId, int ProductID) : IRequest<Models.OrderItems> { }
}
