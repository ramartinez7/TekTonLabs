﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediator.Commands.Order
{
    public record UpdateCommand(Models.Order Order) : IRequest<Models.Order> { }
}
