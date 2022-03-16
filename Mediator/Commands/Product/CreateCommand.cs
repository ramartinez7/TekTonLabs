using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Commands.Product
{
    public record CreateCommand(Models.Product Product) : IRequest<Models.Product> { }
}
