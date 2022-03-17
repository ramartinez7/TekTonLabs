using MediatR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Commands.OrderItem
{
    public record AddOrderItemCommand(Models.OrderItem OrderItem) : IRequest<Models.OrderItem> { }
}
